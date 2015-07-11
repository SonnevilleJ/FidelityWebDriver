using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Pages
{
    public class LoginPage : ILoginPage
    {
        private readonly IWebDriver _webDriver;
        private readonly IPageFactory _pageFactory;

        public LoginPage(IWebDriver webDriver, IPageFactory pageFactory)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;
        }

        public ISummaryPage LogIn(string username, string password)
        {
            InsertTextIntoInput(By.Id("userId-input"), username);
            InsertTextIntoInput(By.Id("password"), password);
            _webDriver.FindElement(By.Id("fs-login-button")).Click();

            return _pageFactory.GetPage<ISummaryPage>();
        }

        private void InsertTextIntoInput(By @by, string username)
        {
            var element = _webDriver.FindElement(@by);
            element.SendKeys(username);
        }
    }
}