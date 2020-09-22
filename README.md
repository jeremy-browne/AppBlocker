# AppBlocker
Blocks a predefined list of apps

## The plan
- PC requests list of blocked apps
- Server returns app rules for that PC
- PC looks at verdict and kills blocks apps as required

# Dev Milestones

## Local App
1. Read processes running on PC
2. Search a predefined list of apps to block
3. Kill blocked apps
4. Tray Icon
5. (Web based?) UI to add, remove applications and set rule times
6. Send requests to webserver

## Web Server
1. Hold extensive list of blocked apps
2. Listen to requests from tray app
3. Return with a verdict of the app being blocked or not
