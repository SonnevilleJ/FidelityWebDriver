using System;
using System.IO;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver.Tests.Pages
{
    [TestFixture]
    public class ActivityPageTests : PageFactoryTests<IActivityPage>
    {
        private ActivityPage _activityPage;

        private Mock<IWebDriver> _webDriverMock;

        private Mock<IPageFactory> _pageFactoryMock;

        private Mock<IWebElement> _downloadLinkMock;

        [SetUp]
        public void Setup()
        {
            _downloadLinkMock = new Mock<IWebElement>();

            _webDriverMock = new Mock<IWebDriver>();
            _webDriverMock.Setup(webDriver => webDriver.FindElement(By.ClassName("activity--history-download-link")))
                .Returns(_downloadLinkMock.Object);

            _pageFactoryMock = new Mock<IPageFactory>();

            _activityPage = new ActivityPage(_webDriverMock.Object, _pageFactoryMock.Object);
        }

        [Test]
        public void DownloadHistoryShouldClickHistoryExpander()
        {
            var filePath = _activityPage.DownloadHistory(DateTime.MinValue, DateTime.MaxValue);

            _downloadLinkMock.Verify(link => link.Click());
            var expectedFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Downloads");
            Assert.AreEqual(expectedFilePath, filePath);
        }
    }
}