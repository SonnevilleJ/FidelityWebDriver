using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Pages
{
    public interface IPageFactory
    {
        T GetPage<T>(IWebDriver webDriver) where T : IPage;
    }
}