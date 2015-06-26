using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Pages
{
    public class HomePage : IHomePage
    {
        private readonly IPageFactory _pageFactory;

        public HomePage(IWebDriver webDriver, IPageFactory pageFactory)
        {
            _pageFactory = pageFactory;
            WebDriver = webDriver;
        }

        public ILoginPage GoToLoginPage()
        {
            WebDriver.FindElement(By.ClassName("pntlt"))
                .FindElement(By.ClassName("pnlogout"))
                .FindElement(By.ClassName("last-child"))
                .FindElement(By.TagName("a"))
                .Click();
            return _pageFactory.GetPage<ILoginPage>();
        }

        public IWebDriver WebDriver { get; private set; }
    }
}