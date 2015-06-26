using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriverTests.Pages
{
    [TestFixture]
    public class HomepageTests
    {
        private IHomePage _homePage;
        private Mock<IWebDriver> _webDriverMock;
        private Mock<IWebElement> _divMock;
        private Mock<IWebElement> _ulMock;
        private Mock<IWebElement> _liMock;
        private Mock<IWebElement> _aMock;

        [SetUp]
        public void Setup()
        {
            _aMock = new Mock<IWebElement>();

            _liMock = new Mock<IWebElement>();
            _liMock.Setup(a => a.FindElement(By.TagName("a"))).Returns(_aMock.Object);

            _ulMock = new Mock<IWebElement>();
            _ulMock.Setup(ul => ul.FindElement(By.ClassName("last-child"))).Returns(_liMock.Object);
            
            _divMock = new Mock<IWebElement>();
            _divMock.Setup(div => div.FindElement(By.ClassName("pnlogout"))).Returns(_ulMock.Object);

            _webDriverMock = new Mock<IWebDriver>();
            _webDriverMock.Setup(driver =>driver.FindElement(By.ClassName("pntlt"))).Returns(_divMock.Object);


            _homePage = new HomePage(_webDriverMock.Object);
        }

        [Test]
        public void ShouldNavigateToLoginPage()
        {
            var loginPage = _homePage.GoToLoginPage();

            _aMock.Verify(a => a.Click());
            Assert.IsNotNull(loginPage);
        }
    }
}
