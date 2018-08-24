#!/usr/bin/env node
var program   = require('commander');
var WebSocket = require('ws');
var url       = require('url');
var sprintf   = require("sprintf-js").sprintf;
var logFormat = require('./format');
var moment    = require('moment');

var default_port = 443;

var target_hosts = {
  'us': 'apilog.kii.com',
  'jp': 'apilog-jp.kii.com',
  'cn': 'apilog-cn2.kii.com',
  'sg': 'apilog-sg.kii.com',
  'cn3': 'apilog-cn3.kii.com',
  'eu': 'apilog-eu.kii.com',
}
var backward_compatible_names = {
  "loginname" : "login-name",
  "phonenumber" : "phone-number",
  "emailaddress" : "email-address",
  "loginName" : "login-name",
  "emailAddress" :  "email-address",
  "phoneNumber" : "phone-number",
  "accountType" : "account-type",
  "newGroupName" : "new-name",
  "newMember" : "new-member",
  "failedToAdd" : "failed-to-add",
  "removedMember" : "remove-member",
  "bucketType" :  "bucket-type",
  "dataType" : "data-type",
  "resultsCount" : "results-count",
  "bodyDataType" : "body-data-type",
  "clientHash" : "client-hash",
  "deletedCount" : "deleted-count",
  "installationType" :  "device-type",
  "filterID" : "filter-id",
  "succeededEndpoints" : "succeeded-endpoints",
  "authHeader" : " auth-header",
  "responseType" : "response-type",
  "failedEndpoints" : "failed-endpoints"
}
var valid_date_formats = ['YYYY-MM-DD', 'YYYY-MM-DD HH:mm:ss', 'YYYY-MM-DD HH:mm'];

// Caching format string and field names per key.
// {
//   "default":{
//     "formatString": "%s [%s] %s %s",
//     "fields": ["time", "level", "key" ,"description"]
//   }
//   "user.registered":{...}
// }
var compiled_format_cache = {};

function generateCacheKey(key, level) {
  if (level == "ERROR" && key != "servercode.log") {
    return key + level;
  }
  return key;
}

function _formatLog(line, useUtc) {
  var format = logFormat[line.key];
  if (format === null) {
    return line;
  }
  if (!format) {
    format = logFormat['default'];
    if (!format)
      return line;
  }
  pristine = true;
  var cacheKey = generateCacheKey(line.key, line.level);
  for(var attributename in line){
//    console.log("debug-log: " + attributename+": "+line[attributename]);
    if (line[attributename] == "null") {
// remove the empty element from json object
      delete line[attributename]
      var regex = new RegExp(" [^\\s]*:\\${" + attributename + "}");
      format = format.replace(regex, "");
// This line format line has been modified, so we need to set a flag in order it won't be cached
      pristine = false;
    } else if (format.indexOf("${"+attributename+"}") < 0) {
      if (attributename !== "_id" && attributename !== "appID" && attributename !== "exception" && attributename !== "internalUserID") {
//         console.log("we need to add " + attributename + " into this: " + format); 
        var name = attributename;
      // make sure to use backward compatible labels
        if (attributename in backward_compatible_names) {
           name = backward_compatible_names[attributename];
        }
        format = format + " " + name + ":${"+attributename+"}";
        pristine = false;
      }
    }
  }
  var compiled_format = compiled_format_cache[cacheKey];
  if (!compiled_format || !pristine) {
    compiled_format = {};
    var reg = /\$\{([^}]+)\}/g;
    // generate format string
    // ex.) "${time} [${level}] ${key} ${description}" => "%s [%s] %s %s"
    compiled_format.formatString = format.replace(reg, "%s");
    var fields = [];
    // In case an error is logged, append exception text
    if (line.level == "ERROR") {
       format = format + " exception:${exception}";
       compiled_format.formatString =  compiled_format.formatString + " exception:%s";
    }
    // extract fieldName and get field values using fieldName.
    // ex.) 1. "${time} [${level}] ${key} ${description}" => ["time", "level", "key", "description"]
    while (fieldName = reg.exec(format)) {
      fields.push(fieldName[1]);
    }
    compiled_format.fields = fields;
// only cache if the format line is pristine
    if (pristine) {
      compiled_format_cache[cacheKey] = compiled_format;
    }
  }
  if (useUtc === undefined) {
     line['time'] = moment.utc(line['time']).local().format('YYYY-MM-DDTHH:mm:ss.SSSZ');
  }
  var args = []; // sprintf's arguments. ('format', arg1, arg2, ...)
  args.push(compiled_format.formatString);
  for (var i = 0; i < compiled_format.fields.length; i++) {
    if (line["key"] == "servercode.log" &&
      compiled_format.fields[i] == "description" &&
      line["description"].indexOf("$STACK_TRACE") >= 0) {
      // if log was outputted by JS-BOX and message contains stack trace
      var m = line["description"].match(/(^.*)\$STACK_TRACE\{(.*)\}$/);
      args.push(m[1] + "\n" + m[2].replace(/,/g, "\n"));
    } else {
      args.push(_to_string(line[compiled_format.fields[i]]));
    }
  }
  return sprintf.apply(this, args);
}

function _create_request(args) {
  var request = {
    appID:        args.appId,
    appKey:       args.appKey,
    clientID:     args.clientId,
    clientSecret: args.clientSecret,
    token:        args.token,
    command:      (args.tail ? 'tail' : 'cat'),
    limit:        args.num,
    userID:       args.userId,
    level:        args.level,
    dateFrom:     (args.dateFrom !== undefined ? (args.useUtc ? 
                      moment.utc(args.dateFrom, valid_date_formats).format('YYYY-MM-DD:HH:mm:ss'): 
                      moment(args.dateFrom, valid_date_formats).utc().format('YYYY-MM-DD:HH:mm:ss')) : args.dateFrom),
    dateTo:       (args.dateTo !== undefined ? (args.useUtc ?
                      moment.utc(args.dateTo, valid_date_formats).format('YYYY-MM-DD:HH:mm:ss'):
                      moment(args.dateTo, valid_date_formats).utc().format('YYYY-MM-DD:HH:mm:ss')) : args.dateTo),
    httpProxy:    args.httpProxy,
  };
  return JSON.stringify(request);
}

function _check_option(cmd, name, commandName) {
  if (!cmd[name]) {
    console.log("Missing param --" + commandName);
    process.exit(1);
  }
}

function _parse_host(site, siteUrl) {
  if (!site && !siteUrl) {
    console.log("Missing param -- site");
    process.exit(1);
  }
  if(siteUrl) {
    var u = url.parse(siteUrl);
    if(['ws:'].indexOf(u.protocol) == -1 && ['wss:'].indexOf(u.protocol) == -1) {
      console.log("Invalid protocol found, protocol must be ws");
      process.exit(1);
    }
    var p = u.protocol.substring(0, u.protocol.length - 1);
    return {
      host: u.hostname,
      protocol: p,
      port: u.port != null ? u.port : default_port,
      path: u.pathname
    };
  } else {
    h = target_hosts[site];
    if(h == null) {
      console.log("Undefined site found: " + site);
      process.exit(1);
    }
    return {
      host: h, protocol: 'wss', port: default_port, path: 'logs'
    }
  }
}

function createAgent(ssl, httpProxy) {
  var agent;
  if (httpProxy) {
    var tunnel = require('tunnel-agent');
    var proxyURL = url.parse(httpProxy);
    var proxy = {
      host: proxyURL.hostname,
      port: proxyURL.port || 8080
    };

    if (ssl) {
      return tunnel.httpsOverHttp({proxy: proxy});
    } else {
      return tunnel.httpOverHttp({proxy: proxy});
    }
  } else {
    return false;
  }
}

function _to_string(val) {
  if (val) {
    return val;
  }
  return "";
}


program
  .option('--site <site>', 'Site to be used. Supported values are: us, jp, cn, sg, cn3, eu.')
  .option('--site-url <url>', 'Full url of the api site. It will prevail over --site. Eg: wss://apilog.kii.com:443/logs')
  .option('--client-id <client-id>', 'Client ID owner of the application. Eg: abcd1234')
  .option('--client-secret <client-secret>', 'Client secret of the application.')
  .option('--token <token>', 'The token to be used. Can be used instead of client-id and client-secret to re-use an existing token')
  .option('--app-id <app-id>', 'App ID of the application. Eg: ab12cd34')
  .option('--app-key <app-key>', 'App Key of the application.')
  .option('-t, --tail', 'Similar to tail -f, realtime tail displays recent logs and leaves the session open for realtime logs to stream in.')
  .option('-n, --num <count>', 'Output the last <count> lines, instead of the last 100; maximum output is 1500 lines.')
  .option('--user-id <user-id>', 'Filtering for a certain user. Eg: 45a678ac-dc51-mvfd-9c1d-aaee48ad0a16')
  .option('--level <level>', 'Filtering for a certain log level. Eg: DEBUG, INFO, ERROR')
  .option('--date-from <from>', 'Filtering from specified start date. Eg: 2013-01-01, 2013-01-01 14:05')
  .option('--date-to <to>', 'Filtering until specified end date. Eg: 2013-01-01, 2013-01-01 14:05')
  .option('--use-utc', 'Use UTC time format for timestamps in output and in date-from and date-to parameters. If not set, local time will be used.')
  .option('--http-proxy <http-proxy>', 'Specify http proxy, in case client is running behind a firewall or cannot access directly to Kii API. Eg: https://my.proxy:8080')
  .parse(process.argv);

if (process.argv.length <= 2) {  // in case of no args
  program.outputHelp()
  process.exit(1)
}
_check_option(program, 'appId', 'app-id');
_check_option(program, 'appKey', 'app-key');
if (!program.token && (!program.clientId || !program.clientSecret)) {
  console.log("Either --token or --client-id / --client-secret pair are required");
  process.exit(1);
}
// To Fix : 'Error: Hostname/IP doesn't match certificate's altnames'
// see https://github.com/awssum/awssum/issues/164
process.env.NODE_TLS_REJECT_UNAUTHORIZED = 0;

var target = _parse_host(program.site, program.siteUrl);

var endpoint = sprintf("%s://%s:%s/%s", target.protocol, target.host, target.port, target.path);
var wsc = new WebSocket(endpoint, { agent: createAgent(target.protocol == 'wss', program.httpProxy) });
wsc.on('error', function(error) {
  console.log(error);
  process.exit(1);
});
wsc.on('open', function() {
  wsc.send(_create_request(program));
});
wsc.on('close', function() {
  process.exit(1);
});
wsc.on('message', function(message) {
  var lines = JSON.parse(message);
  if (Array.isArray(lines)) {
    for (var i = 0; i < lines.length; i++) {
      console.log(_formatLog(lines[i], program.useUtc));
    }
    if (!program.tail) {
      wsc.close();
      process.exit(0);
    }
  } else {
    // if message is not array, message is error object.
    if (lines && lines.err) {
      console.log(lines.err);
    }
    wsc.close();
    process.exit(1);
  }
});
