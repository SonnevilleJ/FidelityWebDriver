using System;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver.Tests
{
    [TestFixture]
    public class SiteNavigatorTests : IDisposable
    {
        private SiteNavigator _siteNavigator;
        private Mock<IWebDriver> _webDriverMock;
        private Mock<INavigation> _navigationMock;
        private Mock<IHomePage> _homePageMock;
        private Mock<IPageFactory> _pageFactoryMock;
        private Mock<ILoginPage> _loginPageMock;
        private Mock<ISummaryPage> _summaryPageMock;

        [SetUp]
        public void Setup()
        {
            SetupWebDriver();
            SetupPageFactory();
            _siteNavigator = new SiteNavigator(_webDriverMock.Object, _pageFactoryMock.Object);
        }

        private void SetupWebDriver()
        {
            _navigationMock = new Mock<INavigation>();

            _webDriverMock = new Mock<IWebDriver>();
            _webDriverMock.Setup(webDriver => webDriver.Navigate()).Returns(_navigationMock.Object);
        }

        private void SetupPageFactory()
        {
            _homePageMock = new Mock<IHomePage>();
            _loginPageMock = new Mock<ILoginPage>();
            _summaryPageMock = new Mock<ISummaryPage>();

            _pageFactoryMock = new Mock<IPageFactory>();
            _pageFactoryMock.Setup(factory => factory.GetPage<IHomePage>()).Returns(_homePageMock.Object);
            _pageFactoryMock.Setup(factory => factory.GetPage<ILoginPage>()).Returns(_loginPageMock.Object);
            _pageFactoryMock.Setup(factory => factory.GetPage<ISummaryPage>()).Returns(_summaryPageMock.Object);
        }

        [Test]
        public void ShouldDisposeWebDriver()
        {
            _siteNavigator.Dispose();

            _webDriverMock.Verify(driver => driver.Dispose());
        }

        [Test]
        public void ShouldGoToHomePage()
        {
            var homePage = _siteNavigator.GoTo<IHomePage>();

            _navigationMock.Verify(navigation => navigation.GoToUrl("https://www.fidelity.com"));
            Assert.AreEqual(_homePageMock.Object, homePage);
        }

        [Test]
        public void ShouldGoToLoginPage()
        {
            var loginPage = _siteNavigator.GoTo<ILoginPage>();

            _homePageMock.Verify(page => page.GoToLoginPage());
            Assert.AreEqual(_loginPageMock.Object, loginPage);
        }

        [Test]
        public void ShouldGoToSummaryPage()
        {
            var summaryPage = _siteNavigator.GoTo<ISummaryPage>();

            _navigationMock.Verify(navigation => navigation.GoToUrl("https://oltx.fidelity.com/ftgw/fbc/oftop/portfolio"));
            Assert.AreEqual(_summaryPageMock.Object, summaryPage);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var siteNavigator = _siteNavigator;
                if (siteNavigator != null)
                {
                    siteNavigator.Dispose();
                    _siteNavigator = null;
                }
            }
        }
    }
}