using System.Collections.Generic;
using System.Security.Authentication;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver.Tests.Pages
{
    [TestFixture]
    public class LoginPageTests : PageFactoryTests<ILoginPage>
    {
        private LoginPage _loginPage;
        private string _username;
        private string _password;
        private Mock<IWebElement> _usernameInputMock;
        private Mock<IWebElement> _passwordInputMock;
        private Mock<IWebElement> _submitButtonMock;
        private Mock<IWebDriver> _webDriverMock;
        private Mock<IPageFactory> _pageFactoryMock;
        private Mock<ISummaryPage> _summaryPageMock;
        private List<IWebElement> _errorDivs;

        [SetUp]
        public void Setup()
        {
            _username = "username";
            _password = "password";

            _usernameInputMock = new Mock<IWebElement>();

            _passwordInputMock = new Mock<IWebElement>();

            _errorDivs = new List<IWebElement>();

            _submitButtonMock = new Mock<IWebElement>();

            _webDriverMock = new Mock<IWebDriver>();
            _webDriverMock.Setup(driver => driver.FindElement(By.Id("userId-input"))).Returns(_usernameInputMock.Object);
            _webDriverMock.Setup(driver => driver.FindElement(By.Id("password"))).Returns(_passwordInputMock.Object);
            _webDriverMock.Setup(driver => driver.FindElement(By.Id("fs-login-button"))).Returns(_submitButtonMock.Object);
            _webDriverMock.Setup(webDriver => webDriver.FindElements(By.ClassName("error-page")))
                .Returns(_errorDivs.AsReadOnly());

            _summaryPageMock = new Mock<ISummaryPage>();

            _pageFactoryMock = new Mock<IPageFactory>();
            _pageFactoryMock.Setup(pageFactory => pageFactory.GetPage<ISummaryPage>())
                .Returns(_summaryPageMock.Object);

            _loginPage = new LoginPage(_webDriverMock.Object, _pageFactoryMock.Object);
        }

        [Test]
        public void ShouldSubmitWithCredentials()
        {
            _submitButtonMock.Setup(button => button.Click()).Callback(() =>
            {
                _usernameInputMock.Verify(input => input.SendKeys(_username), "Must enter username before clicking submit!");
                _passwordInputMock.Verify(input => input.SendKeys(_password), "Must enter password before clicking submit!");
            });

            var summaryPage = _loginPage.LogIn(_username, _password);

            _submitButtonMock.Verify(button => button.Click());

            Assert.AreSame(_summaryPageMock.Object, summaryPage);
        }

        [Test]
        [ExpectedException(typeof(InvalidCredentialException))]
        public void ShouldThrowIfCredentialsFail()
        {
            _submitButtonMock.Setup(button => button.Click())
                .Callback(() => _errorDivs.Add(new Mock<IWebElement>().Object));

            _loginPage.LogIn("Superman", "Who needs tools?");
        }
    }
}