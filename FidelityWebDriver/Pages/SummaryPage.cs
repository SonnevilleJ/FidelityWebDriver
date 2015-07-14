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

        public double GetGainLossAmount()
        {
            var text = _webDriver.FindElement(By.ClassName("js-today-change-value-dollar")).Text;

            return double.Parse(text, NumberStyles.Currency);
        }

        public double GetGainLossPercent()
        {
            var text = _webDriver.FindElement(By.ClassName("js-today-change-value-percent")).Text;

            var tempt = text.Replace("(", "").Replace(")", "").Replace(" ", "");
            var result = tempt.Replace(CultureInfo.CurrentCulture.NumberFormat.PercentSymbol, "");

            return double.Parse(result)/100.0;
        }

        public IPositionsPage GoToPositionsPage()
        {
            _webDriver.FindElement(By.CssSelector("[data-tab-name='Positions']")).Click();

            return _pageFactory.GetPage<IPositionsPage>();
        }
    }
}