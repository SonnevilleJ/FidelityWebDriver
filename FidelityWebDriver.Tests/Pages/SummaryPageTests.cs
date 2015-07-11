using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver.Tests.Pages
{
    [TestFixture]
    public class SummaryPageTests : PageFactoryTests<ISummaryPage>
    {
        private SummaryPage _summaryPage;
        private Mock<IWebDriver> _webDriverMock;
        private Mock<IPageFactory> _pageFactoryMock;
        private double _balanceNumber;
        private Mock<IWebElement> _fullBalanceSpan;

        [SetUp]
        public void Setup()
        {
            _balanceNumber = 1234.56;

            _fullBalanceSpan = new Mock<IWebElement>();
            _fullBalanceSpan.Setup(span => span.Text).Returns(_balanceNumber.ToString("C"));

            _webDriverMock = new Mock<IWebDriver>();
            _webDriverMock.Setup(driver => driver.FindElement(By.ClassName("js-total-balance-value")))
                .Returns(_fullBalanceSpan.Object);

            _pageFactoryMock = new Mock<IPageFactory>();

            _summaryPage = new SummaryPage(_webDriverMock.Object, _pageFactoryMock.Object);
        }

        [Test]
        public void ShouldReturnBalanceOfAllAccounts()
        {
            var balance = _summaryPage.GetBalanceOfAllAccounts();

            Assert.AreEqual(_balanceNumber, balance);
        }
    }
}