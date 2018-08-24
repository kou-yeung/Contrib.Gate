#!/usr/bin/env node
// set TMP
if(process.env.TMP || process.env.TEMP) {
  process.env['LOGPATH'] = process.env.TMP ? process.env.TMP + '/node.log' : process.env.TEMP + '/node.log'; 
}

var program = require('commander'),
    kii = require('../lib/kiiCommand');


executeCommand();

function executeCommand() {
  program
    .option('--site <site>', 'Site to be used. Eg: us, jp, cn, sg, cn3, eu.')
    .option('--site-url <url>', 'Full url of the api site. It will prevail over --site. Eg: https://api.kii.com/api')
    .option('--app-id <app-id>', 'App ID of the application. Eg: ab12cd34')
    .option('--app-key <app-key>', 'App Key of the application. Eg: jidmkljk1409kkdfhr21234iofew')
    .option('--client-id <client-id>', 'Client ID owner of the application. Eg: bc9d0209fkog')
    .option('--client-secret <client-secret>', 'Client secret of the owner of the application. Eg: kjlamcaduhjnqdcoadf88ijhfhundfadfg')
    .option('--token <token>', 'The token to be used. Can be used instead of client-id and client-secret to re-use an existing token')
    .option('--debug', 'Enable verbose mode')
    .option('--http-proxy <http-proxy>', 'Specify http proxy, in case client is running behind a firewall or cannot access directly to Kii API. Eg: https://my.proxy:8080');

  program.command('deploy-file')
    .option('--file <file>', 'Script to be deployed. Eg: /tmp/my-file.js')
    .option('--hook-config <file>', 'Hook configuration file to be deployed. Eg: /tmp/my-hook-config.js')
    .option('--set-current', 'Set the newly-deployed versions as the current version')
    .option('--environment-version <environmentVersion>', 'Environment version of which the deploy code runs.')
    .action(function(cmd) {
      kii.deployFile(program, cmd);
    });

  // program.command('configure')
  //   .option('--site <site>', 'Site to be used. Eg: us, jp, cn.')
  //   .option('--site-url <url>', 'Full url of the api site. It will prevail over --site. Eg: https://api.kii.com/api')
  //   .option('--app-id <app-id>', 'App ID of the application. Eg: ab12cd34')
  //   .option('--app-key <app-key>', 'App Key of the application. Eg: jidmkljk1409kkdfhr21234iofew')
  //   .option('--client-id <client-id>', 'Client ID owner of the application. Eg: bc9d0209fkog')
  //   .option('--client-secret <client-secret>', 'Client secret of the owner of the application. Eg: kjlamcaduhjnqdcoadf88ijhfhundfadfg')
  //   .action(function(cmd) {
  //     kii.configure(program, cmd);
  //   });

  program.command('get')
    .option('--code-version <version>', 'Version to be obtained. Eg: v12354')
    .option('--output-file <output-file>', 'Output file. Eg: /tmp/whatever.js')
    .action(function(cmd) {
      kii.downloadFile(program, cmd);
    });

  program.command('get-hook-config')
    .option('--code-version <version>', 'Version to be obtained. Eg: v12354')
    .option('--output-file <output-file>', 'Output file. Eg: /tmp/whatever.js')
    .action(function(cmd) {
      kii.downloadHookConfigFile(program, cmd);
    });

  program.command('list')
    .option('--utc-time', 'Enable UTC time in output dates')
    .action(function(cmd) {
      kii.listFiles(program, cmd);
    });

  program.command('delete')
    .option('--code-version <version>', 'Version to be deleted. Eg: v12354')
    .action(function(cmd) {
      kii.deleteFile(program, cmd);
    });

  program.command('set-current')
    .option('--code-version <version>', 'Version to be set to current. Eg: v12354')
    .action(function(cmd){
      kii.setCurrent(program,cmd);
    })

  program.command('list-scheduled-execution')
    .option('--from <date>', "Start of the interval in which the execution started. Eg: '2013-02-08 09:30'")
    .option('--to <date>', "End of the interval in which the execution started (Current time if omitted). Eg: '2013-02-08 10:30'")
    .option('--utc-time', 'Enable UTC time in both input and output dates')
    .action(function(cmd) {
      kii.listExecutions(program, cmd);
    });

  program.command('get-scheduled-execution')
    .option('--id <id>', 'Schedule execution id. Eg: ym3hopxch6srpkdv7e7jswdcd')
    .action(function(cmd) {
      kii.getExecution(program, cmd);
    });

  program.parse(process.argv);

  // show help in case no command/params were found
  if (!program.args.length)
    program.help(); 
  // show help in case no command was found
  program.args.forEach(function(a) {
   if(typeof a == 'string')
      program.help(); 
  });
}
