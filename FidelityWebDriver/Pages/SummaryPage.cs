using System.Globalization;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Pages
{
    public class SummaryPage : ISummaryPage
    {
        private readonly IWebDriver _webDriver;
        private readonly IPageFactory _pageFactory;

        public SummaryPage(IWebDriver webDriver, IPageFactory pageFactory)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;
        }

        public double GetBalanceOfAllAccounts()
        {
            var balanceText = _webDriver.FindElement(By.ClassName("js-total-balance-value")).Text;

            return double.Parse(balanceText, NumberStyles.Currency);
        }
    }
}