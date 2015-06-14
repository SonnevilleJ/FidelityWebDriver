using Ninject.Modules;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Sonneville.TradingHistoryCsvDownloader.Ninject
{
    public class SeleniumModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IWebDriver>().To<ChromeDriver>();
        }
    }
}