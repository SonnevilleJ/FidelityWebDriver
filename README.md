# Project Overview
FidelityWebDriver is a wrapper around the Selenium WebDriver, used to perform semantic driving of the Fidelity website.

Current build status: ![Build Status](http://sonnevillej.ddns.net:9000/app/rest/builds/buildType:(id:FidelityWebDriver_Build)/statusIcon)

Much of the demo app and library is still not functional, but login capability has been implemented.

## Storing credentials
Use the [SetCredentials()][SetCredentials] test to store Fidelity credentials in your Windows user account. This prevents them from being stored in the repository and accidentally checked in.

[SetCredentials]: https://github.com/SonnevilleJ/FidelityWebDriver/blob/master/FidelityWebDriver.Demo.Tests/AppTests.cs#L72
