using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.CSV;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver.Tests.Pages
{
    [TestFixture]
    public abstract class PageFactoryTests<T> where T : IPage
    {
        private PageFactory _factory;
        private Mock<IWebDriver> _driverMock;
        private Mock<ICsvDownloadService> _csvDownloadServiceMock;

        [SetUp]
        public void SetupPageFactory()
        {
            _driverMock = new Mock<IWebDriver>(MockBehavior.Strict);

            _csvDownloadServiceMock = new Mock<ICsvDownloadService>();

            _factory = new PageFactory(_driverMock.Object, _csvDownloadServiceMock.Object);
        }

        [Test]
        public void ShouldReturnSamePageForEachRequest()
        {
            var page1 = _factory.GetPage<T>();
            var page2 = _factory.GetPage<T>();

            Assert.AreSame(page1, page2);
        }
    }
}