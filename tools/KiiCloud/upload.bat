@echo on

SET CurrentPath=%~dp0

call %CurrentPath%/../loadenv %CurrentPath%/setting.ini

SET KiiPath=%CurrentPath%/kii-cli-v1.5.1
SET JSPath=%CurrentPath%/../../app/server/Contrib.Gate/server.js

node %KiiPath%/bin/kii-servercode.js deploy-file ^
 --file %JSPath% ^
 --site jp ^
 --app-id %your_app_id% ^
 --app-key %your_app_key% ^
 --client-id %your_client_id% ^
 --client-secret %your_client_secret% ^
 --set-current
