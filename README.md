# Project Overview
FidelityWebDriver is a wrapper around the Selenium WebDriver, used to perform semantic driving of the Fidelity website.

Current build status: ![Build Status](http://sonnevillej.ddns.net:9000/app/rest/builds/buildType:(id:FidelityWebDriver_Build)/statusIcon)

Much of the demo app and library is still not functional, but login capability has been implemented. While I'm still building out the structure of the library, I am planning on accept pull requests in the future.

## Demo app
_This project depends on ChromeDriver, so be sure you have Chrome installed!_

The demo app will use the specified credentials to log into the www.Fidelity.com website and print some basic account info to the console.

## Command line arguments
```
  -u, --username=VALUE       the username to use when logging into Fidelity.
  -p, --password=VALUE       the password to use when logging into Fidelity.
  -s, --save                 indicates options should be persisted.
  -h, --help                 shows this message and exits.
```
If the --save argument is given, settings are stored to [Isolated Storage](https://msdn.microsoft.com/en-us/library/3ak841sy(v=vs.110).aspx). Passwords are encrypted at rest.
