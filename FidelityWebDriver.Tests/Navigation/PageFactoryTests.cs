using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Navigation;
using Sonneville.FidelityWebDriver.Positions;
using Sonneville.FidelityWebDriver.Transactions;
using Sonneville.Utilities;

namespace Sonneville.FidelityWebDriver.Tests.Navigation
{
    [TestFixture]
    public abstract class PageFactoryTests<T> where T : IPage
    {
        private PageFactory _factory;
        private Mock<IWebDriver> _driverMock;
        private Mock<IAccountSummariesExtractor> _accountSummariesExtractorMock;
        private Mock<IAccountDetailsExtractor> _accountDetailsExtractorMock;
        private Mock<ISleepUtil> _sleepUtilMock;
        private Mock<IHistoryTransactionParser> _historyTransactionParserMock;

        [SetUp]
        public void SetupPageFactory()
        {
            _driverMock = new Mock<IWebDriver>(MockBehavior.Strict);

            _accountSummariesExtractorMock = new Mock<IAccountSummariesExtractor>();

            _accountDetailsExtractorMock = new Mock<IAccountDetailsExtractor>();

            _sleepUtilMock = new Mock<ISleepUtil>();

            _historyTransactionParserMock = new Mock<IHistoryTransactionParser>();
            _factory = new PageFactory(
                _driverMock.Object,
                _accountSummariesExtractorMock.Object,
                _accountDetailsExtractorMock.Object,
                _sleepUtilMock.Object,
                _historyTransactionParserMock.Object);
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