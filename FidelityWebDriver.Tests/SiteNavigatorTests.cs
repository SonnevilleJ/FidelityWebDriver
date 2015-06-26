using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver.Tests
{
    [TestFixture]
    public class SiteNavigatorTests
    {
        private SiteNavigator _siteNavigator;
        private Mock<IWebDriver> _webDriverMock;
        private Mock<INavigation> _navigationMock;
        private Mock<IHomePage> _homePageMock;
        private Mock<IPageFactory> _pageFactoryMock;

        [SetUp]
        public void Setup()
        {
            _homePageMock = new Mock<IHomePage>();

            _navigationMock = new Mock<INavigation>();

            _webDriverMock = new Mock<IWebDriver>();
            _webDriverMock.Setup(webDriver => webDriver.Navigate()).Returns(_navigationMock.Object);

            _pageFactoryMock = new Mock<IPageFactory>();
            _pageFactoryMock.Setup(factory => factory.GetPage<IHomePage>(_webDriverMock.Object)).Returns(_homePageMock.Object);

            _siteNavigator = new SiteNavigator(_webDriverMock.Object, _pageFactoryMock.Object);
        }

        [Test]
        public void ShouldOpenHomePage()
        {
            var resultPage = _siteNavigator.GoToHomePage();

            Assert.AreEqual(_homePageMock.Object, resultPage);
            _navigationMock.Verify(navigation => navigation.GoToUrl("https://www.fidelity.com"));
        }

        [Test]
        public void ShouldDisposeWebDriver()
        {
            _siteNavigator.Dispose();

            _webDriverMock.Verify(driver => driver.Dispose());
        }

        [Test]
        public void ShouldOpenLoginPage()
        {
            var loginPage = _siteNavigator.GoToLoginPage();

            _homePageMock.Verify(page => page.GoToLoginPage());
            Assert.IsNotNull(loginPage);
        }
    }
}