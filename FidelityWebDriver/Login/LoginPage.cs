using System.Linq;
using System.Security.Authentication;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Login
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

            if (_webDriver.FindElements(By.ClassName("error-page")).Any())
            {
                throw new InvalidCredentialException("Failed to log into Fidelity.com with given credentials!");
            }
            return _pageFactory.GetPage<ISummaryPage>();
        }

        private void InsertTextIntoInput(By @by, string username)
        {
            var element = _webDriver.FindElement(@by);
            element.SendKeys(username);
        }
    }
}