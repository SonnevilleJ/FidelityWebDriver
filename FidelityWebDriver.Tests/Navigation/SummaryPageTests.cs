using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Navigation;
using Sonneville.FidelityWebDriver.Positions;
using Sonneville.FidelityWebDriver.Transactions;

namespace Sonneville.FidelityWebDriver.Tests.Navigation
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

        private Mock<IWebElement> _activityLi;
        private Mock<IActivityPage> _activityPageMock;
        private Mock<IWebElement> _progressBarDiv;

        [SetUp]
        public void Setup()
        {
            SetupPageFactory();

            _balanceNumber = 1234.56;

            _fullBalanceSpan = new Mock<IWebElement>();
            _fullBalanceSpan.Setup(span => span.Text).Returns(_balanceNumber.ToString("C"));

            _gainLossAmount = 12.35;

            _gainLossAmountSpan = new Mock<IWebElement>();
            _gainLossAmountSpan.Setup(span => span.Text).Returns(_gainLossAmount.ToString("C"));

            _gainLossPercent = 0.1;

            _gainLossPercentSpan = new Mock<IWebElement>();
            _gainLossPercentSpan.Setup(span => span.Text)
                .Returns($"(+{_gainLossPercent:P})");

            _positionsLi = new Mock<IWebElement>();

            _progressBarDiv = new Mock<IWebElement>();
            _progressBarDiv.Setup(div => div.Displayed).Returns(() =>
            {
                try
                {
                    return true;
                }
                finally
                {
                    _progressBarDiv.Setup(div => div.Displayed).Returns(false);
                }
            });

            _activityLi = new Mock<IWebElement>();
            _activityLi.Setup(li => li.Click()).Callback(() =>
            {
                Assert.IsFalse(_progressBarDiv.Object.Displayed, "Progress bar div is obstructing element!");
            });

            _webDriverMock = new Mock<IWebDriver>();
            _webDriverMock.Setup(driver => driver.FindElement(By.ClassName("js-total-balance-value")))
                .Returns(_fullBalanceSpan.Object);
            _webDriverMock.Setup(driver => driver.FindElement(By.CssSelector("[data-tab-name='Positions']")))
                .Returns(_positionsLi.Object);
            _webDriverMock.Setup(driver => driver.FindElement(By.CssSelector("[data-tab-name='Activity']")))
                .Returns(_activityLi.Object);
            _webDriverMock.Setup(driver => driver.FindElement(By.ClassName("js-today-change-value-dollar")))
                .Returns(_gainLossAmountSpan.Object);
            _webDriverMock.Setup(driver => driver.FindElement(By.ClassName("js-today-change-value-percent")))
                .Returns(_gainLossPercentSpan.Object);
            _webDriverMock.Setup(webDriver => webDriver.FindElement(By.ClassName("progress-bar")))
                .Returns(_progressBarDiv.Object);

            _positionsPageMock = new Mock<IPositionsPage>();

            _activityPageMock = new Mock<IActivityPage>();
            
            _pageFactoryMock = new Mock<IPageFactory>();
            _pageFactoryMock.Setup(factory => factory.GetPage<IPositionsPage>()).Returns(_positionsPageMock.Object);
            _pageFactoryMock.Setup(factory => factory.GetPage<IActivityPage>()).Returns(_activityPageMock.Object);

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

        [Test]
        public void ShouldNavigateToActivityPage()
        {
            var activityPage = _summaryPage.GoToActivityPage();

            _activityLi.Verify(li => li.Click());
            Assert.AreSame(_activityPageMock.Object, activityPage);
        }
    }
}