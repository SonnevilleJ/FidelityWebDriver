using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Navigation;
using Sonneville.FidelityWebDriver.Positions;

namespace Sonneville.FidelityWebDriver.Tests.Positions
{
    [TestFixture]
    public class PositionsPageTests
    {
        private PositionsPage _positionsPage;
        private Mock<IWebDriver> _webDriverMock;
        private Mock<IPageFactory> _pageFactoryMock;
        private Mock<IAccountSummariesExtractor> _positionsPageAccountsExtractorMock;

        [SetUp]
        public void Setup()
        {
            _webDriverMock = new Mock<IWebDriver>();

            _pageFactoryMock = new Mock<IPageFactory>();

            _positionsPageAccountsExtractorMock = new Mock<IAccountSummariesExtractor>();

            _positionsPage = new PositionsPage(_webDriverMock.Object, _pageFactoryMock.Object,
                _positionsPageAccountsExtractorMock.Object);
        }

        [Test]
        public void ShouldReturnExtractedSummaries()
        {
            var expectedSummaries = new List<IAccount>();
            _positionsPageAccountsExtractorMock
                .Setup(extractor => extractor.ExtractAccountSummaries(_webDriverMock.Object))
                .Returns(expectedSummaries);

            var actualSummaries = _positionsPage.GetAccountSummaries();

            Assert.AreSame(expectedSummaries, actualSummaries);
        }
    }
}