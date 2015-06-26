using Ninject.Modules;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Sonneville.FidelityWebDriver.Demo.Ninject
{
    public class SeleniumModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IWebDriver>().To<ChromeDriver>().InSingletonScope();
        }
    }
}