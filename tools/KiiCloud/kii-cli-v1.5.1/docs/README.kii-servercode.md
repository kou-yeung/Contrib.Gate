kii-servercode
==============

Every command execution requires at least the following context information: site, app-id, app-key, client-id and client-secret or token.
Currently this context information needs to be passed in every execution, but this should change in the future.

## Install
```
npm install
sudo npm link
```

Note: If you are using Node.js 0.12 (or greater), there will be a warning regarding failed installation of `execSync` dependency. Since Node.js 0.12 (and greater) have this package already builtin, the warning can be safely ignored.

## Basic Usage

  Usage: kii-servercode.js [options] [command]

  Commands:

    deploy-file [options] 
    get [options]         
    get-hook-config [options]
    list                  
    delete [options]     
    set-current [options]
    list-scheduled-execution [options] 
    get-scheduled-execution [options] 

  Options:

    -h, --help                       output usage information
    --site <site>                    Site to be used. Eg: us, jp, cn, sg, cn3, eu.
    --site-url <url>                 Full url of the api site. It will prevail over --site. Eg: https://api.kii.com/api
    --app-id <app-id>                App ID of the application. Eg: ab12cd34
    --app-key <app-key>              App Key of the application. Eg: jidmkljk1409kkoqerqpokjd234iofew
    --client-id <client-id>          Client ID owner of the application. Eg: bc9d0209fkog
    --client-secret <client-secret>  Client secret of the owner of the application. Eg: kjlamcaduhjnqi92jdn18adf88ijhfhundfadfg
    --token <token>                  The token to be used. Can be used instead of client-id and client-secret to re-use an existing token
    --debug                          Enable verbose mode
    --http-proxy                     Specify http proxy, in case client is running behind a firewall or cannot access directly to Kii API. Eg: https://my.proxy:8080

## Deploy a File

  Usage: deploy-file [options]

  Options:

    -h, --help     output usage information
    --file <file>  Script to be deployed. Eg: /tmp/my-file.js
    --hook-config <file>  Hook configuration file to be deployed. Eg: /tmp/my-hook-config.js
    --set-current  Set the newly-deployed versions as the current version
    --environment-version Specify the environment version server-code runs on. Currnt supported environemnt versions are 0 and 6. 6 is encouraged.

  Example:

    node kii-servercode.js deploy-file --file demo.js ----environment-version 6 \
    --site us --app-id demoapp --app-key jhhjd87y8yi --client-id 898weerih9e98 --client-secret ugh08wyiuhdskjhsdjf

## Set Current Version

  Usage: set-current [options]

  Options:

    -h, --help  output usage information
    --code-version <version>  Version to be set as current. Eg: v12354

  Example:

    node kii-servercode.js set-current --code-version ezk1qjrqo64ynkkpsyc7zsud9 \
    --site us --app-id demoapp --app-key jhhjd87y8yi --client-id 898weerih9e98 --client-secret ugh08wyiuhdskjhsdjf

## Get a File

  Usage: get [options]

  Options:

    -h, --help                   output usage information
    --code-version <version>     Version to be obtained. Eg: v12354
    --output-file <output-file>  Output file. Eg: /tmp/whatever.js

  Example:

    node kii-servercode.js get --code-version hvfejfhvg \
    --site us --app-id demoapp --app-key jhhjd87y8yi --client-id 898weerih9e98 --client-secret ugh08wyiuhdskjhsdjf

## Get a Hook Config File

  Usage: get-hook-config [options]

  Options:

    -h, --help                   output usage information
    --code-version <version>     Version to be obtained. Eg: v12354
    --output-file <output-file>  Output file. Eg: /tmp/whatever.js

  Example:

    node kii-servercode.js get --code-version hvfejfhvg \
    --site us --app-id demoapp --app-key jhhjd87y8yi --client-id 898weerih9e98 --client-secret ugh08wyiuhdskjhsdjf

## List Available Versions

  Usage: list [options]

  Options:

    -h, --help  output usage information
    --utc-time  Enable UTC time in output dates

  Example:

    node kii-servercode.js list \
    --site us --app-id demoapp --app-key jhhjd87y8yi --client-id 898weerih9e98 --client-secret ugh08wyiuhdskjhsdjf

## Delete a File

  Usage: delete [options]

  Options:

    -h, --help                output usage information
    --code-version <version>  Version to be deleted. Eg: v12354

  Example:

    node kii-servercode.js delete --code-version ghfdgfdg \
    --site us --app-id demoapp --app-key jhhjd87y8yi --client-id 898weerih9e98 --client-secret ugh08wyiuhdskjhsdjf

## List schedule executions

  Usage: list-scheduled-execution [options]

  Options:

    -h, --help     output usage information
    --from <date>  Start of the interval in which the execution started. Eg: '2013-02-08 09:30'
    --to <date>    End of the interval in which the execution started (Current time if omitted). Eg: '2013-02-08 10:30'
    --utc-time     Enable UTC time in both input and output dates


  Example:

    node kii-servercode.js list-scheduled-execution --from 2013-02-08 \
    --site us --app-id demoapp --app-key jhhjd87y8yi --client-id 898weerih9e98 --client-secret ugh08wyiuhdskjhsdjf

## Get a schedule execution

  Usage: get-scheduled-execution [options]

  Options:

    -h, --help  output usage information
    --id <id>  Schedule execution id. Eg: ym3hopxch6srpkdv7e7jswdcd

  Example:

    node kii-servercode.js get-scheduled-execution --id ghfdgfdg \
    --site us --app-id demoapp --app-key jhhjd87y8yi --client-id 898weerih9e98 --client-secret ugh08wyiuhdskjhsdjf
