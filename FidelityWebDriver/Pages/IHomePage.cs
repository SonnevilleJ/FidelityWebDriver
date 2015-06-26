using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Pages
{
    public interface IHomePage : IPage
    {
        ILoginPage GoToLoginPage();
    }
}