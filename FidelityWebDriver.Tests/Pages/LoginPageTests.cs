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
        private Mock<IWebDriver> _webDriverMock;
        private string _username;
        private string _password;
        private Mock<IWebElement> _usernameInputMock;
        private Mock<IWebElement> _passwordInputMock;
        private Mock<IWebElement> _submitButtonMock;

        [SetUp]
        public void Setup()
        {
            _username = "username";
            _password = "password";

            _usernameInputMock = new Mock<IWebElement>();

            _passwordInputMock = new Mock<IWebElement>();

            _submitButtonMock = new Mock<IWebElement>();

            _webDriverMock = new Mock<IWebDriver>();
            _webDriverMock.Setup(driver => driver.FindElement(By.Id("userId-input"))).Returns(_usernameInputMock.Object);
            _webDriverMock.Setup(driver => driver.FindElement(By.Id("password"))).Returns(_passwordInputMock.Object);
            _webDriverMock.Setup(driver => driver.FindElement(By.Id("fs-login-button"))).Returns(_submitButtonMock.Object);

            _loginPage = new LoginPage(_webDriverMock.Object);
        }

        [Test]
        public void ShouldSubmitWithCredentials()
        {
            _submitButtonMock.Setup(button => button.Click()).Callback(() =>
            {
                _usernameInputMock.Verify(input => input.SendKeys(_username), "Must enter username before clicking submit!");
                _passwordInputMock.Verify(input => input.SendKeys(_password), "Must enter password before clicking submit!");
            });
            
            _loginPage.LogIn(_username, _password);

            _submitButtonMock.Verify(button => button.Click());
        }
    }
}