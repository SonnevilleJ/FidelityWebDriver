using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver
{
    public class FidelityDriver : IFidelityDriver
    {
        private readonly IWebDriver _webDriver;

        public FidelityDriver(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public IHomePage GoToHomepage()
        {
            _webDriver.Navigate().GoToUrl("https://www.fidelity.com");

            return new HomePage(null);
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