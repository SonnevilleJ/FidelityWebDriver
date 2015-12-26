using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Login;

namespace Sonneville.FidelityWebDriver.Navigation
{
    public class HomePage : IHomePage
    {
        private readonly IWebDriver _webDriver;
        private readonly IPageFactory _pageFactory;

        public HomePage(IWebDriver webDriver, IPageFactory pageFactory)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;
        }

        public ILoginPage GoToLoginPage()
        {
            _webDriver.FindElement(By.ClassName("pntlt"))
                .FindElement(By.ClassName("pnlogout"))
                .FindElement(By.ClassName("last-child"))
                .FindElement(By.TagName("a"))
                .Click();
            return _pageFactory.GetPage<ILoginPage>();
        }
    }
}