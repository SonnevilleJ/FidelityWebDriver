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
        private Mock<IPositionsPage> _positionsPageMock;
        private Mock<IWebElement> _positionsLi;
        private Mock<IWebElement> _gainLossAmountSpan;
        private Mock<IWebElement> _gainLossPercentSpan;
        private double _gainLossAmount;
        private double _gainLossPercent;

        [SetUp]
        public void Setup()
        {
            _balanceNumber = 1234.56;

            _fullBalanceSpan = new Mock<IWebElement>();
            _fullBalanceSpan.Setup(span => span.Text).Returns(_balanceNumber.ToString("C"));

            _gainLossAmount = 12.35;

            _gainLossAmountSpan = new Mock<IWebElement>();
            _gainLossAmountSpan.Setup(span => span.Text).Returns(_gainLossAmount.ToString("C"));

            _gainLossPercent = 0.1;

            _gainLossPercentSpan = new Mock<IWebElement>();
            _gainLossPercentSpan.Setup(span => span.Text)
                .Returns(string.Format("(+{0})", _gainLossPercent.ToString("P")));

            _positionsLi = new Mock<IWebElement>();

            _webDriverMock = new Mock<IWebDriver>();
            _webDriverMock.Setup(driver => driver.FindElement(By.ClassName("js-total-balance-value")))
                .Returns(_fullBalanceSpan.Object);
            _webDriverMock.Setup(driver => driver.FindElement(By.CssSelector("[data-tab-name='Positions']")))
                .Returns(_positionsLi.Object);
            _webDriverMock.Setup(driver => driver.FindElement(By.ClassName("js-today-change-value-dollar")))
                .Returns(_gainLossAmountSpan.Object);
            _webDriverMock.Setup(driver => driver.FindElement(By.ClassName("js-today-change-value-percent")))
                .Returns(_gainLossPercentSpan.Object);

            _positionsPageMock = new Mock<IPositionsPage>();
            
            _pageFactoryMock = new Mock<IPageFactory>();
            _pageFactoryMock.Setup(factory => factory.GetPage<IPositionsPage>()).Returns(_positionsPageMock.Object);

            _summaryPage = new SummaryPage(_webDriverMock.Object, _pageFactoryMock.Object);
        }

        [Test]
        public void ShouldReturnBalanceOfAllAccounts()
        {
            var balance = _summaryPage.GetBalanceOfAllAccounts();

            Assert.AreEqual(_balanceNumber, balance);
        }

        [Test]
        public void ShouldReturnTodaysGainLossAmount()
        {
            var gainLossAmount = _summaryPage.GetGainLossAmount();

            Assert.AreEqual(_gainLossAmount, gainLossAmount);
        }

        [Test]
        public void ShouldReturnTodaysGainLossPercent()
        {
            var gainLossPercent = _summaryPage.GetGainLossPercent();

            Assert.AreEqual(_gainLossPercent, gainLossPercent);
        }

        [Test]
        public void ShouldNavigateToPositionsPage()
        {
            var positionsPage = _summaryPage.GoToPositionsPage();

            _positionsLi.Verify(li => li.Click());
            Assert.AreSame(_positionsPageMock.Object, positionsPage);
        }
    }
}