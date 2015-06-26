using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver
{
    public class SiteNavigator : ISiteNavigator
    {
        private readonly IWebDriver _webDriver;
        private readonly IPageFactory _pageFactory;

        public SiteNavigator(IWebDriver webDriver, IPageFactory pageFactory)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;
        }

        public IHomePage GoToHomePage()
        {
            _webDriver.Navigate().GoToUrl("https://www.fidelity.com");

            return _pageFactory.GetPage<IHomePage>(_webDriver);
        }

        public ILoginPage GoToLoginPage()
        {
            GoToHomePage().GoToLoginPage();
            return new LoginPage();
        }

        public IActivityPage GoToActivityPage()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var webDriver = _webDriver;
                if (webDriver != null) webDriver.Dispose();
            }
        }
    }
}