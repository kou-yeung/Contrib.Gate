var path = require('path'),
    fs = require('fs'),
    url = require('url'),
    util = require('util');

var hosts = {
  'us': 'api.kii.com',
  'jp': 'api-jp.kii.com',
  'cn': 'api-cn2.kii.com',
  'sg': 'api-sg.kii.com',
  'cn3': 'api-cn3.kii.com',
  'eu': 'api-eu.kii.com',
}

// ~ exports

exports.loadContext = function(program) {
  return loadContext(program);
}

exports.parseOptions = function(program) {
  return parseOptions(program);
}

exports.checkOption = function(cmd, name, commandName) {
  checkOption(cmd, name, commandName);
}

exports.storeConfig = function(ctx) {
  storeConfig(ctx);
}

// ~ real stuff

function loadContext(program) {
  // var ctx = loadConfig();
  var ctx = null;
  if(!ctx || hasConfigProp(program))
    ctx = parseOptions(program);
  // set paths
  ctx['basePath'] = '/apps/' + ctx.appID + '/server-code';
  ctx['versionsPath'] = '/apps/' + ctx.appID + '/server-code/versions';
  ctx['hookBasePath'] = '/apps/' + ctx.appID + '/hooks';
  ctx['hookVersionsPath'] = '/apps/' + ctx.appID + '/hooks/versions';
  ctx['httpProxy'] = program.httpProxy; 
  ctx['debug'] = program.debug != null && program.debug == true;
    
  return ctx;
}

function hasConfigProp(program) {
  var props = ['appId', 'appKey', 'site', 'siteUrl', 'clientId', 'clientSecret'];
  for(var i = 0; i < props.length; i++) {
    if(program.hasOwnProperty(props[i]))
      return true;
  }
  return false;
}

function parseOptions(program) {
  if(!program.site && !program.siteUrl)
    exit("Either --site or --site-url is required");

  checkOption(program, 'appId', 'app-id');
  checkOption(program, 'appKey', 'app-key');

  if(!program.token && (!program.clientId || !program.clientSecret))
    exit("Either --token or --client-id / --client-secret pair are required");

  return {
    url:          parseHost(program.site, program.siteUrl),
    appID:        program.appId,
    appKey:       program.appKey,
    token:        program.token,
    clientID:     program.clientId,
    clientSecret: program.clientSecret
  };
}

function checkOption(cmd, name, commandName) {
  if (!cmd[name])
    exit("Missing param --" + commandName);
}

function parseHost(site, siteUrl) {
  if(siteUrl != null) {
    var u = url.parse(siteUrl);
    if(['http:', 'https:'].indexOf(u.protocol) == -1) 
      exit("Invalid protocol found, indicate either http or https in site url " + siteUrl);
     
    var p = u.protocol.substring(0, u.protocol.length - 1);
    return {
      host:     u.hostname,
      protocol: p,
      port:     u.port != null ? u.port : (p == 'http' ? 80 : 443),
      path:     u.pathname
    };
  } else {
    h = hosts[site];
    if(h == null)
      exit("Undefined site found: " + site);
      
    return {
      host: h, protocol: 'https', port: 443, path: '/api'
    }
  } 
}

function storeConfig(ctx) {
  var config = getConfigPath();
  if(!config)
    exit('Unable to config app credentials in your system.');  

  util.log('Storing app credentials in ' + config);
  fs.writeFileSync(config, JSON.stringify(ctx, null, 2), 'utf-8');
}

function loadConfig() {
  var config = getConfigPath();
  if(!config || !fs.existsSync(config))
    return {};

  var content = fs.readFileSync(config, 'utf-8');
  try {
    return JSON.parse(content);
  } catch (err) {
    console.error('Unable to parse config file. Check file content at ' + config);
    return null;
  }
}

function getConfigPath() {
  var config;
  if(process.env.HOME)
    config = path.join(process.env.HOME, '.kiicmd');
  else if(process.platform.match(/^win/) && process.env.USERPROFILE)
    config = path.join(process.env.USERPROFILE, 'Application Data', 'kiicmd.ini')
  return config;
}

function exit(message) {
  console.error(message);
  process.exit(1);
}
