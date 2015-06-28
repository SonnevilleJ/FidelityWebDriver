using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Pages
{
    public class LoginPage : ILoginPage
    {
        public LoginPage(IWebDriver webDriver)
        {
            WebDriver = webDriver;
        }

        public void LogIn(string username, string password)
        {
            InsertTextIntoInput(By.Id("userId-input"), username);
            InsertTextIntoInput(By.Id("password"), password);
            WebDriver.FindElement(By.Id("fs-login-button")).Click();
        }

        private void InsertTextIntoInput(By @by, string username)
        {
            var element = WebDriver.FindElement(@by);
            element.SendKeys(username);
        }

        public IWebDriver WebDriver { get; private set; }
    }
}