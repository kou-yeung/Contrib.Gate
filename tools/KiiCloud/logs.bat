@echo on

SET CurrentPath=%~dp0

call %CurrentPath%/../loadenv %CurrentPath%/setting.ini

SET KiiPath=%CurrentPath%/kii-cli-v1.5.1
SET JSPath=%CurrentPath%/../../app/server/Contrib.Gate/app.js

node %KiiPath%/bin/kii-logs.js -t ^
 --site jp ^
 --app-id %your_app_id% ^
 --app-key %your_app_key% ^
 --client-id %your_client_id% ^
 --client-secret %your_client_secret%

 pause