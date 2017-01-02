using System;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Sonneville.FidelityWebDriver.Navigation
{
    public class SummaryPage : ISummaryPage
    {
        private readonly IWebDriver _webDriver;

        public SummaryPage(IWebDriver webDriver)
        {
            _webDriver = webDriver;
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

        public void GoToPositionsPage()
        {
            WaitUntilNotDisplayed(_webDriver, By.ClassName("progress-bar"));

            _webDriver.FindElement(By.CssSelector("[data-tab-name='Positions']")).Click();
            WaitUntilNotDisplayed(_webDriver, By.ClassName("progress-bar"));
        }

        public void GoToActivityPage()
        {
            WaitUntilNotDisplayed(_webDriver, By.ClassName("progress-bar"));

            _webDriver.FindElement(By.CssSelector("[data-tab-name='Activity']")).Click();
            WaitUntilNotDisplayed(_webDriver, By.ClassName("progress-bar"));
        }

        private void WaitUntilNotDisplayed(IWebDriver webDriver, By element)
        {
            new WebDriverWait(webDriver, TimeSpan.FromMinutes(1))
                .Until(driver => !driver.FindElement(element).Displayed);
        }
    }
}