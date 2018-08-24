var https = require('https'),
    http = require('http'),
    util = require('util'),
    url = require('url');

exports.login = function(ctx, callback) {
  login(ctx, callback);
}

exports.send = function(ctx, path, method, headers, body, callback, callbackError) {
  send(ctx, path, method, headers, body, callback, callbackError);
}

exports.sendForm = sendForm;

function login(ctx, callback) {
  if(ctx.token != null) {
    if(ctx.debug)
      util.log('Using token ' + ctx.token + ' for authentication');
    return callback(ctx);
  }

  if(ctx.debug)
    util.log("Using client-id = " + ctx.clientID + " and client-secret = " 
      + ctx.clientSecret + " for authentication");

  var tokenRequest = JSON.stringify({
    'client_id': ctx.clientID,
    'client_secret': ctx.clientSecret
  }, null, 2);

  var options = {
    hostname: ctx.url.host, port: ctx.url.port, path: normalize(ctx.url.path) + '/oauth2/token',
    method: 'POST',
    agent: createAgent(ctx.url.protocol == "https", ctx.httpProxy),
    headers: {
      'X-Kii-AppID': ctx.appID, 'X-Kii-AppKey': ctx.appKey,
      'Content-Type' : 'application/json',
      'Content-Length': tokenRequest.length
    }
  };

  if(ctx.debug) {
    util.log("REQUEST:\n" + JSON.stringify(options, null, 2));
    util.log("REQUEST body:\n" + tokenRequest);
  }

  var req = client(ctx.url.protocol).request(options, function(response) {
    response.setEncoding('utf-8');

    var responseBody = "";
    response.on('data', function(data) { responseBody += data; });

    response.on('end', function() {
      if(ctx.debug) {     
        util.log('RESPONSE status: ' + response.statusCode);
        util.log('RESPONSE headers:\n' + JSON.stringify(response.headers, null, 2));
        util.log('RESPONSE body: \n' + responseBody);
      }

      if(hasError(response)) {
        console.error('Failed to authenticate: ' + response.statusCode + ' - (' + responseBody + ')');
        console.error('Check your app-id, app-key, client-id & client-secret.');
        console.error('Execution context: ' + JSON.stringify(ctx, null, 2));
        process.exit(1);
      }
      ctx.token = JSON.parse(responseBody)['access_token'];
      callback(ctx);
    });
  });

  req.on('error', errorLogger);

  req.write(tokenRequest);
  req.end();
}

function sendForm(ctx, path, method, headers, form, callback, callbackError) {
  var options = makeOptions(ctx, path, method, headers);
  var formHeaders = form.getHeaders();
  for (var k in formHeaders) {
    options.headers[k] = formHeaders[k]
  }

  if(ctx.debug) {
    util.log("REQUEST: \n" + JSON.stringify(options, null, 2));
  }
  var handler = sendResponseHandler(ctx, callback, callbackError);
  var req = client(ctx.url.protocol).request(options, handler);
  form.pipe(req)

  req.on('error', errorLogger);
  req.end();
}


function send(ctx, path, method, headers, body, callback, callbackError) {
  var contentLength = body != null ? Buffer.byteLength(body, 'utf8') : 0;
  var options = makeOptions(ctx, path, method, headers, contentLength);
  
  if(ctx.debug) {
    util.log("REQUEST: \n" + JSON.stringify(options, null, 2));
    if(body != null)
      util.log("REQUEST body: \n" + body);
  }

  var handler = sendResponseHandler(ctx, callback, callbackError);
  var req = client(ctx.url.protocol).request(options, handler);

  req.on('error', errorLogger);

  if(body != null)
    req.write(body, 'utf8');

  req.end();
}

/** utils */

function hasError(response) {
  return response.statusCode > 399;
}

function errorLogger(e) { 
  console.error('Error found while serving request: ' + e.message + ' - ' + e.stack); 
}

function client(protocol) {
  return protocol == 'http' ? http : https;
}

function normalize(path) {
  return path.substring(path.length - 1) == '/' ?
    path.substring(0, path.length -1) : path;
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

/** Make common request options. */
function makeOptions(ctx, path, method, headers, contentLength) {
  var options = {
    hostname: ctx.url.host, port: ctx.url.port, path: normalize(ctx.url.path) + path,
    method: method,
    agent: createAgent(ctx.url.protocol == "https", ctx.httpProxy),
    headers: {
      'X-Kii-AppID': ctx.appID, 'X-Kii-AppKey': ctx.appKey,
      'Authorization': 'Bearer ' + ctx.token
    }
  };

  if (contentLength) {
    options.headers['Content-Length'] = contentLength
  }
  for (var k in headers) {
    options.headers[k] = headers[k];
  }

  return options;
}

/** Make common response handler */
function sendResponseHandler(ctx, callback, callbackError) {
  var handler = function(response) {
    var responseBody = "";
    response.on('data', function(data) { responseBody += data; });

    response.on('end', function() {
      if(ctx.debug) {
        util.log('RESPONSE status: ' + response.statusCode);
        util.log('RESPONSE headers:\n' + JSON.stringify(response.headers, null, 2));      
        util.log('RESPONSE body: \n' + responseBody);
      }

      if(hasError(response)) {
        if (callbackError != null) {
          callbackError(response.statusCode);
        } else {
          var jsonType = /json/g;
          var mediaType = response.headers['content-type'];
          
          if(jsonType.test(mediaType)) 
            console.error('Error found: ' + responseBody);
          else 
            console.error('Http error found: http code ' + response.statusCode + ' - body: ' + responseBody);
          
          process.exit(1);
        }
      } else {
        callback(responseBody);
      }
    })
  }
  return handler;
}

