# Project Overview
FidelityWebDriver is a wrapper around the [Selenium WebDriver](http://www.seleniumhq.org/projects/webdriver/), used to perform semantic driving of the Fidelity website.

## Current build status
MSBuild: ![Build Status](http://sonnevillej.privatedns.org:9000/app/rest/builds/buildType:(id:FidelityWebDriver_MSBuild)/statusIcon)
Mono: ![Build Status](http://sonnevillej.privatedns.org:9000/app/rest/builds/buildType:(id:FidelityWebDriver_Mono)/statusIcon)

# Demo app
_The demo app depends on [Docker](https://www.docker.com/), so be sure you have Docker installed!_

The demo app will use the specified credentials to log into the www.Fidelity.com website. Once logged in, it will print to the console some basic account info as well as the most recent transactions.

## Launching the demo
1. Start a Selenium docker container with the following command:
`docker run -d -p 4444:4444 selenium/standalone-chrome`
2. Complie the solution and run Sonneville.FidelityWebDriver.Demo.exe with any of the below parameters:
```
  -u, --username=VALUE       the username to use when logging into Fidelity.
  -p, --password=VALUE       the password to use when logging into Fidelity.
  -s, --save                 indicates options should be persisted to demo.ini file.
  -h, --help                 shows this message and exits.
```
If the --save argument is given, settings are stored to `demo.ini` allowing future demo executions to omit the `-u` and `-p` parameters.

# These managers work for you!
To use FidelityWebDriver, identify the IManager implementation which provides the functionality you'd like to consume. Instantiate the manager and required classes using your choice of IWebDriver implementation. An IOC library like [Ninject](http://www.ninject.org/) is recommended. The [Demo app module](https://github.com/SonnevilleJ/FidelityWebDriver/blob/master/FidelityWebDriver.Demo/Ninject/AppModule.cs) can be used to automatically bind all required classes.

Available managers   | Description |
-------------------- | ----------------------------------------------------
Login Manager        | manages login state for the www.Fidelity.com website
Positions Manager    | parses current positions
Transactions Manager | parses previous transactions
