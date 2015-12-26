using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Navigation;
using Sonneville.FidelityWebDriver.Positions;
using Sonneville.FidelityWebDriver.Transactions.CSV;

namespace Sonneville.FidelityWebDriver.Tests.Navigation
{
    [TestFixture]
    public abstract class PageFactoryTests<T> where T : IPage
    {
        private PageFactory _factory;
        private Mock<IWebDriver> _driverMock;
        private Mock<ICsvDownloadService> _csvDownloadServiceMock;
        private Mock<IPositionsPageAccountsExtractor> _positionsPageAccountsExtractorMock;

        [SetUp]
        public void SetupPageFactory()
        {
            _driverMock = new Mock<IWebDriver>(MockBehavior.Strict);

            _csvDownloadServiceMock = new Mock<ICsvDownloadService>();

            _positionsPageAccountsExtractorMock = new Mock<IPositionsPageAccountsExtractor>();

            _factory = new PageFactory(_driverMock.Object, _csvDownloadServiceMock.Object,
                _positionsPageAccountsExtractorMock.Object);
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