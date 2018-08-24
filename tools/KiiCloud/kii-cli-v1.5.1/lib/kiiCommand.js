var fs = require('fs'),
    util = require('util'),
    url = require('url'),
    moment = require('moment'),
    sprintf = require("sprintf-js").sprintf,
    cfg = require('./kiiConfig'),
    formData = require('form-data'),
    http = require('./kiiHttp');

var dateFormat = 'YYYY-MM-DD HH:mm:ss';
var dateFormatWithTZ = dateFormat + ' ZZ';

exports.deployFile = function(program, cmd) {
  var ctx = cfg.loadContext(program);
  cfg.checkOption(cmd, 'file', 'file');

  http.login(ctx, function(context) {
    deployFiles(context, cmd.file, cmd.hookConfig, cmd.setCurrent, cmd.environmentVersion);
  }); 
}

exports.setCurrent = function(program, cmd) {
  var ctx = cfg.loadContext(program);
  cfg.checkOption(cmd, 'codeVersion', 'code-version');
  checkVersion(cmd.codeVersion);
  
  http.login(ctx, function(context) {
    setCurrent(context, cmd.codeVersion);
  });
}

exports.downloadFile = function(program, cmd) {
  var ctx = cfg.loadContext(program);
  cfg.checkOption(cmd, 'codeVersion', 'code-version');
  checkVersion(cmd.codeVersion);
  
  http.login(ctx, function(context) {
    downloadFile(context, cmd.codeVersion, cmd.outputFile);
  });
}

exports.downloadHookConfigFile = function(program, cmd) {
  var ctx = cfg.loadContext(program);
  cfg.checkOption(cmd, 'codeVersion', 'code-version');
  checkVersion(cmd.codeVersion);
  
  http.login(ctx, function(context) {
    downloadHookConfigFile(context, cmd.codeVersion, cmd.outputFile);
  });
}

exports.listFiles = function(program, cmd) {
  var ctx = cfg.loadContext(program);
  
  http.login(ctx, function(context) {
    listFiles(context, cmd.utcTime);
  });
}

exports.deleteFile = function(program, cmd) {
  var ctx = cfg.loadContext(program);
  cfg.checkOption(cmd, 'codeVersion', 'code-version');
  checkVersion(cmd.codeVersion);
  
  http.login(ctx, function(context) {
    deleteFile(context, cmd.codeVersion, deleteHookConfigFile);
  });
}

exports.listExecutions = function(program, cmd) {
  var ctx = cfg.loadContext(program);
  cfg.checkOption(cmd, 'from', 'from');
  
  http.login(ctx, function(context) {
    listExecutions(context, cmd.from, cmd.to, cmd.utcTime);
  });
}

exports.getExecution = function(program, cmd) {
  var ctx = cfg.loadContext(program);
  cfg.checkOption(cmd, 'id', 'id');
  
  http.login(ctx, function(context) {
    getExecution(context, cmd.id);
  });
}

exports.configure = function(program, cmd) {
  var ctx = cfg.parseOptions(program);

  util.log("Configuring app credentials: ");
  util.log(JSON.stringify(ctx, null, 2));

  http.login(ctx, function(context) {
    configure(ctx);
  });
}

/** server code commands */

function configure(ctx) {
  cfg.storeConfig(ctx);
}

function deployFiles(ctx, codeFile, hookConfig, isSetToCurrent, environmentVersion) {
  var codeData;
  try {
    codeData = fs.readFileSync(codeFile, 'utf8');
  } catch(err) { exit("Failed to open file. " + err); }
  
  var configData;
  try {
    configData = hookConfig ? fs.readFileSync(hookConfig, 'utf8') : null;
  } catch(err) { exit("Failed to open file. " + err); }
  
  if (configData) {
    // deploy file, then deploy hooks, then set current. 
    // That way the current version will be set if none of the previous steps fail
    deployCode(ctx, codeData, environmentVersion, function(ctx, versionID) {
      deployHookConfig(ctx, configData, versionID, function() {
        if (isSetToCurrent) 
          setCurrent(ctx, versionID);
      });
    });
  } else {
    deployCode(ctx, codeData, environmentVersion, (isSetToCurrent ? setCurrent: null));
  }
}

function deployCode(ctx, file, environmentVersion, callback) {
  util.log('Deploying file...');
  var contentType;
  var body;
  var headers;
  var sendFunc;

  if (environmentVersion) {
    var form = new formData();
    var metaData = JSON.stringify({'environmentVersion': environmentVersion});
    form.append("metadata", metaData, {
            contentType: "application/vnd.kii.ServerCodeMetadataRequest+json"
    });
    form.append("server-code", file, {
            contentType: "application/javascript"
    });
    body = form;
    sendFunc = http.sendForm
  } else {
    util.log('Deployment without environment-version is deprecated. Please specify --environment-version');
    body = file;
    contentType = 'application/javascript'
    headers = { 'Content-Type' : contentType };
    sendFunc = http.send
  }

  sendFunc(ctx, ctx.basePath, 'POST', headers, body, function(responseBody) {
    var versionID = JSON.parse(responseBody)['versionID'];
    util.log('File deployed as version ' + versionID);
    if(callback != null)
      callback(ctx, versionID);
  });
}

function deployHookConfig(ctx, hookConfig, versionID, callback) {
  util.log('Deploying hook config...');
  var headers = { 'Content-Type' : 'application/vnd.kii.HooksDeploymentRequest+json' };

  http.send(ctx, ctx.hookVersionsPath + '/' + versionID, 'PUT', headers, hookConfig, 
    function(responseBody) {
      util.log('Hook Config deployed at version ' + versionID);
      if(callback != null)
        callback(ctx, versionID);
  });
}

function downloadFile(ctx, version, outputFile) {
  util.log('Downloading code version ' + version + '...');
  http.send(ctx, ctx.versionsPath + '/' + version, 'GET', {}, null, function(responseBody) {   
    if(outputFile) {
      fs.writeFile(outputFile, responseBody, function(err) {
        if(err) {
          exit('Error accessing output file. ' + err);
        } else {
          util.log("Code version written to " + outputFile);
        }
      }); 
    } else {
      util.log("Downloaded content: \n" + responseBody);
    }
  });
}

function downloadHookConfigFile(ctx, version, outputFile) {
  util.log('Downloading hook config version ' + version + '...');
  http.send(ctx, ctx.hookVersionsPath + '/' + version, 'GET', {}, null, function(responseBody) {   
    if(outputFile) {
      fs.writeFile(outputFile, responseBody, function(err) {
        if(err) {
          exit('Error accessing output file. ' + err);
        } else {
          util.log("Hook config version written to " + outputFile);
        }
      }); 
    } else {
      util.log("Downloaded content: \n" + responseBody);
    }
  });
}

function listFiles(ctx, utcTime) {
  util.log('Listing available versions...');
  http.send(ctx, ctx.versionsPath, 'GET', {}, null, function(responseBody) {
    var res = JSON.parse(responseBody);
    util.log("Found " + res.versions.length + " versions: ");
    if(res.versions.length != 0) {
      var pattern = "%-26.26s%-20.20s%s";
      var now = utcTime ? moment.utc() : moment();
      var tz =  utcTime ? " (UTC)" : " (LOCAL)";
      
      console.log(sprintf(pattern, 'ID', 'CREATED' + tz, 'STATUS'));
      console.log(sprintf(pattern, "-", "-", "--------").replace(/\s/gm, '-'));
      // print list of versions
      res.versions.forEach(function(v, i, array) {
        console.log(sprintf(pattern, 
          v.versionID, formatDate(v.modifiedAt, utcTime), (v.current ? "active" : "inactive")));
      });
    }
  });
}

function deleteFile(ctx, version, callback) {
  util.log('Removing code version ' + version + '...');

  http.send(ctx, ctx.versionsPath + '/' + version, 'DELETE', {}, null, function(responseBody) {
    util.log('Version ' + version + ' removed');
    if(callback != null)
      callback(ctx, version);
  });
}

function deleteHookConfigFile(ctx, version) {
  util.log('Trying to remove hook config version ' + version + '...');

  http.send(ctx, ctx.hookVersionsPath + '/' + version, 'DELETE', {}, null, 
    function(responseBody) {
      util.log('Hook config for version ' + version + ' removed');
    },
    function(status) {
      if (status == 404)
        util.log('No hook config for version ' + version + ' found.');
      else {
        console.error('Error while deleting hooks for version '  + version + ' - error code: ' + status);
      }
    });
}

function setCurrent(ctx, version) {     
  util.log('Setting current version to ' + version + '...');
  var headers = { 'Content-Type' : 'text/plain' }; 

  http.send(ctx, ctx.versionsPath + '/current', 'PUT', headers, version, function(responseBody) {
    util.log('Current version set to ' + version);    
  });
}

function listExecutions(ctx, from, to, utcTime) {

  var start = utcTime ? moment.utc(from) : moment(from);
  var end = utcTime ? (to ? moment.utc(to) : moment.utc()) 
    : (to ? moment(to) : moment());

  if(!start.isValid())
    exit("Invalid date format for param `--from': " + from);
  if(!end.isValid())
    exit("Invalid date format for param `--to': " + to);
  if(start.isAfter(end))
    exit("Invalid date range: `--from': " + from + " `--to:'" + to + ". Lower limit exceeds upper limit");

  util.log(sprintf('Listing hook executions from %s to %s...', 
    start.format(dateFormatWithTZ), end.format(dateFormatWithTZ)));
  
  var total = 0;
  var pattern = "%-26.26s%-20.20s%-20.20s%-8.8s%s";
  var url = ctx.hookBasePath + '/executions/query';
  var headers = { 'Content-Type' : 'application/vnd.kii.ScheduleExecutionQueryRequest+json' }; 
  var q = {
    scheduleExecutionQuery: {
      clause: {
        type: 'range',
        field: 'startedAt',
        lowerLimit: start.valueOf(),
        lowerIncluded: true,
        upperLimit: end.valueOf(),
        upperIncluded: true
      },
      orderBy: 'startedAt',
      descending: false
    }
  };

  function doQuery(query) {
    http.send(ctx, url, 'POST', headers, JSON.stringify(query), function(responseBody) {
      var res = JSON.parse(responseBody);
      // print page of results      
      total += res.results.length;
      res.results.forEach(function(s, i, array) {
        console.log(sprintf(pattern, 
          s.scheduleExecutionID, formatDate(s.startedAt, utcTime), formatDate(s.finishedAt, utcTime), s.status, s.name));
      });
      // ask for more data
      if(res.nextPaginationKey != null) {
        query.paginationKey = res.nextPaginationKey;
        doQuery(query);
      } else { util.log ("Total of " + total + " hook(s) were found.")}
    });
  }

  // print header
  var tz =  utcTime ? " (UTC)" : " (LOCAL)";
  console.log(sprintf(pattern, "ID", "STARTED" + tz, "FINISHED" + tz, "STATUS", "JOB NAME"));
  console.log(sprintf(pattern, "-", "-", "-", "-", "--------").replace(/\s/gm, '-'));
  // start query
  doQuery(q);
}

function getExecution(ctx, id) {
  util.log('Getting execution for id ' + id + '...');
  var url = ctx.hookBasePath + '/executions/' + id;
  
  http.send(ctx, url, 'GET', {}, null, function(responseBody) {
    console.log(responseBody);
  });  
}

/** utils */

function formatDate(timestamp, utc) {
  if(isNaN(timestamp))
    return "";
  var date = utc ? moment.utc(timestamp) : moment(timestamp)
  return date.format(dateFormat);
}

function checkVersion(version) {
  if (version.toLowerCase() == 'current') 
    exit("`current' as version identifer is not supported");
}

function exit(message) {
  console.error(message);
  process.exit(1);
}
