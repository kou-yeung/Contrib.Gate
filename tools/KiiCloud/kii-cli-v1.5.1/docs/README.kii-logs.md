# README

kii-logs
=======
The Developer Logs command line tool provides an easy and comfortable way to access detailed log information of various events and actions happening to your application when interacting with Kii Cloud. This will help you to directly analyze and profile usage of your application with Kii Cloud much faster. 
## Install
```sh
npm install
sudo npm link
```

## Basic Usage

  Usage: node kii-logs.js [options]

  Options:

* `-h, --help` 				Output usage information.
* `--site <site>` 			Site to be used. Supported values are: us, jp, cn, sg, cn3, eu.
* `--app-id <app-id>` 			App ID of the application.
* `--app-key <app-key>` 		App Key of the application.
* `--client-id <client-id>` 		Client ID of the application.
* `--client-secret <client-secret>` 	Client Secret of the application.
* `--token <token>` 			The token to be used. Can be used instead of client-id and client-secret to re-use an existing token
* `-t, --tail` 				Leave log session open for realtime logs to stream in.
* `-n, --num <count>` 			Output the last <count> lines, instead of the last 100; maximum output is 1500 lines.
* `--user-id <user-id>` 		Filtering for a certain user. Eg: 45a678ac-dc51-mvfd-9c1d-aaee48ad0a16
* `--level <level>` 			Filtering for a certain log level. Eg: DEBUG, INFO, ERROR
* `--date-from <from>`			Filtering from specified start date. Eg: 2013-01-01, 2013-01-01 14:05
* `--date-to <to>`			Filtering until specified end date. Eg: 2013-01-31, 2013-01-31 14:05
* `--use-utc`				Use UTC time format for timestamps in output and in date-from and date-to parameters. If not set, local time will be used.
* `--http-proxy <http-proxy>`           Specify http proxy, in case client is running behind a firewall or cannot access directly to Kii API. Eg: https://my.proxy:8080

Example:

    node kii-logs.js -t \
      --n 500 \
      --app-id abcd1234 \
      --app-key jidmkljk1409kkoqerqpokjd234iless \
      --client-id 34f9eff771524c27cbd1234dff7fb19 \
      --client-secret 4b1580123b2b91af1c6bf5epyf9ba861fc56974ea90feab0e31c25f80f8eb546 \
      --site us

