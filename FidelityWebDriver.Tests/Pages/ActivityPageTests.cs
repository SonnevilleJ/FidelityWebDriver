using System;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.CSV;
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
        private Mock<ICsvDownloadService> _downloadServiceMock;
        private string _fileContents;

        [SetUp]
        public void Setup()
        {
            _fileContents = "file contents";
            _downloadServiceMock = new Mock<ICsvDownloadService>();
            _downloadServiceMock.Setup(service => service.GetDownloadedContent()).Returns(_fileContents);

            _downloadLinkMock = new Mock<IWebElement>();
            _downloadLinkMock.Setup(link => link.Click())
                .Callback(() => _downloadServiceMock.Verify(service => service.Cleanup(), Times.Once()));

            _webDriverMock = new Mock<IWebDriver>();
            _webDriverMock.Setup(webDriver => webDriver.FindElement(By.ClassName("activity--history-download-link")))
                .Returns(_downloadLinkMock.Object);

            _pageFactoryMock = new Mock<IPageFactory>();

            _activityPage = new ActivityPage(_webDriverMock.Object, _pageFactoryMock.Object, _downloadServiceMock.Object);
        }

        [Test]
        public void DownloadHistoryShouldClickHistoryExpander()
        {
            var actualContents = _activityPage.DownloadHistory(DateTime.MinValue, DateTime.MaxValue);

            Assert.AreEqual(_fileContents, actualContents);
            _downloadServiceMock.Verify(service => service.Cleanup(), Times.Exactly(2));
        }
    }
}