using System;
using Ninject.Modules;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Sonneville.FidelityWebDriver.Demo.log4net;

namespace Sonneville.FidelityWebDriver.Demo.Ninject
{
    public class SeleniumModule : NinjectModule
    {
        public override void Load()
        {
            try
            {
                var remoteAddress = new Uri("http://localhost:4444/wd/hub");
                var actualWebDriver = new RemoteWebDriver(remoteAddress, DesiredCapabilities.Chrome());
                Kernel.Bind<IWebDriver>()
                    .ToConstant(actualWebDriver)
                    .WhenInjectedExactlyInto<LoggingWebDriver>()
                    .InSingletonScope();

                Kernel.Bind<IWebDriver>()
                    .To<LoggingWebDriver>()
                    .InSingletonScope();
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: Failed to initialize WebDriver: {e}");
            }
        }
    }
}
