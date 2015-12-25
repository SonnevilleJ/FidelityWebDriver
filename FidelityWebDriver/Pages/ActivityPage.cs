using System;
using System.IO;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Pages
{
    public class ActivityPage : IActivityPage
    {
        private readonly IWebDriver _webDriver;

        private readonly IPageFactory _pageFactory;

        public ActivityPage(IWebDriver webDriver, IPageFactory pageFactory)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;
        }

        public string DownloadHistory(DateTime minValue, DateTime maxValue)
        {
            var downloadLink = _webDriver.FindElement(By.ClassName("activity--history-download-link"));
            downloadLink.Click();

            var documentsFolder = Environment.SpecialFolder.MyDocuments;
            var folderPath = Environment.GetFolderPath(documentsFolder);
            var downloadsPath = Path.Combine(folderPath, "Downloads");
            return downloadsPath;
        }
    }
}