using System;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.CSV;

namespace Sonneville.FidelityWebDriver.Pages
{
    public class ActivityPage : IActivityPage
    {
        private readonly IWebDriver _webDriver;
        private readonly IPageFactory _pageFactory;
        private readonly ICsvDownloadService _csvDownloadService;

        public ActivityPage(IWebDriver webDriver, IPageFactory pageFactory, ICsvDownloadService csvDownloadService)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;
            _csvDownloadService = csvDownloadService;
        }

        public string DownloadHistory(DateTime minValue, DateTime maxValue)
        {
            _csvDownloadService.Cleanup();
            var downloadLink = _webDriver.FindElement(By.ClassName("activity--history-download-link"));
            downloadLink.Click();

            var content = _csvDownloadService.GetDownloadedContent();
            _csvDownloadService.Cleanup();
            return content;
        }
    }
}