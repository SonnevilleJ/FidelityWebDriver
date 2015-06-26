using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Pages
{
    public class PageFactory : IPageFactory
    {
        public T GetPage<T>(IWebDriver webDriver) where T : IPage
        {
            throw new System.NotImplementedException();
        }
    }
}