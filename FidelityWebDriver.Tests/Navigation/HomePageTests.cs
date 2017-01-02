using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Login;
using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Tests.Navigation
{
    [TestFixture]
    public class HomePageTests : PageFactoryTests<IHomePage>
    {
        private IHomePage _homePage;
        private Mock<IWebElement> _divMock;
        private Mock<IWebElement> _ulMock;
        private Mock<IWebElement> _liMock;
        private Mock<IWebElement> _aMock;
        private Mock<IWebDriver> _webDriverMock;
        private Mock<IPageFactory> _pageFactoryMock;
        private Mock<ILoginPage> _loginPageMock;

        [SetUp]
        public void Setup()
        {
            SetupPageFactory();

            _aMock = new Mock<IWebElement>();

            _liMock = new Mock<IWebElement>();
            _liMock.Setup(a => a.FindElement(By.TagName("a"))).Returns(_aMock.Object);

            _ulMock = new Mock<IWebElement>();
            _ulMock.Setup(ul => ul.FindElement(By.ClassName("last-child"))).Returns(_liMock.Object);
            
            _divMock = new Mock<IWebElement>();
            _divMock.Setup(div => div.FindElement(By.ClassName("pnlogout"))).Returns(_ulMock.Object);

            _webDriverMock = new Mock<IWebDriver>();
            _webDriverMock.Setup(driver =>driver.FindElement(By.ClassName("pntlt"))).Returns(_divMock.Object);

            _loginPageMock = new Mock<ILoginPage>();

            _pageFactoryMock = new Mock<IPageFactory>();
            _pageFactoryMock.Setup(factory => factory.GetPage<ILoginPage>()).Returns(_loginPageMock.Object);

            _homePage = new HomePage(_webDriverMock.Object, _pageFactoryMock.Object);
        }

        [Test]
        public void ShouldNavigateToLoginPage()
        {
            var loginPage = _homePage.GoToLoginPage();

            _aMock.Verify(a => a.Click());
            Assert.AreSame(_loginPageMock.Object, loginPage);
        }
    }
}
