using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Pages
{
    public class HomePage : IHomePage
    {
        private readonly IWebDriver _webDriver;

        public HomePage(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public ILoginPage GoToLoginPage()
        {
            _webDriver.FindElement(By.ClassName("pntlt"))
                .FindElement(By.ClassName("pnlogout"))
                .FindElement(By.ClassName("last-child"))
                .FindElement(By.TagName("a"))
                .Click();
            return new LoginPage();
        }
    }
}