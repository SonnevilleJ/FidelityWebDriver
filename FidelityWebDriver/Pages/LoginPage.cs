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
            throw new System.NotImplementedException();
        }

        public IWebDriver WebDriver { get; private set; }
    }
}