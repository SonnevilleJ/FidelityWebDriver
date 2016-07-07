using System;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Navigation;
using Sonneville.FidelityWebDriver.Transactions.CSV;
using Sonneville.Utilities;

namespace Sonneville.FidelityWebDriver.Transactions
{
    public class ActivityPage : IActivityPage
    {
        private readonly IWebDriver _webDriver;
        private readonly IPageFactory _pageFactory;
        private readonly ICsvDownloadService _csvDownloadService;
        private readonly ISleepUtil _sleepUtil;

        public ActivityPage(IWebDriver webDriver, IPageFactory pageFactory, ICsvDownloadService csvDownloadService, ISleepUtil sleepUtil)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;
            _csvDownloadService = csvDownloadService;
            _sleepUtil = sleepUtil;
        }

        public string DownloadHistory(DateTime startDate, DateTime endDate)
        {
            ThrowIfDateRangeIsInvalid(startDate, endDate);

            _csvDownloadService.Cleanup();
            var historyExpanderLink = _webDriver.FindElement(By.Id("historyExpander"));
            historyExpanderLink.Click();

            var rangeDropdown = _webDriver.FindElement(By.Id("activity--history-range-dropdown"));
            rangeDropdown.FindElement(By.CssSelector("option[value='custom']")).Click();

            var dateRangeDiv = _webDriver.FindElement(By.ClassName("activity--history-custom-date-container"));
            var fromDateInput = dateRangeDiv.FindElement(By.ClassName("activity--history-custom-date-from-field"));
            fromDateInput.SendKeys(startDate.ToString("MM/dd/yyyy"));
            var toDateInput = dateRangeDiv.FindElement(By.ClassName("activity--history-custom-date-to-field"));
            toDateInput.SendKeys(endDate.ToString("MM/dd/yyyy"));
            var setTimePeriodButton = dateRangeDiv.FindElement(By.ClassName("activity--history-custom-date-display-button"));
            setTimePeriodButton.Click();

            _sleepUtil.Sleep(500);
            var downloadLink = _webDriver.FindElement(By.ClassName("activity--history-download-link"));
            downloadLink.Click();

            var content = _csvDownloadService.GetDownloadedContent();
            _csvDownloadService.Cleanup();
            return content;
        }

        private void ThrowIfDateRangeIsInvalid(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("Start Date must preceed End Date!");
            if (endDate - startDate > TimeSpan.FromDays(90))
                throw new ArgumentException("Date range must be less than or equal to 90 days!");
            if (endDate.Date > DateTime.Today)
                throw new ArgumentException("Cannot query for future dates!");
        }
    }
}