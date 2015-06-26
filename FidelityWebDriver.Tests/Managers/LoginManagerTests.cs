using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Managers;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver.Tests.Managers
{
    [TestFixture]
    public class LoginManagerTests : ManagerTestsBase<LoginManager>
    {
        private Mock<ILoginPage> _loginPageMock;
        private string _username;
        private string _password;

        protected override LoginManager InstantiateManager()
        {
            return new LoginManager(SiteNavigatorMock.Object);
        }

        [SetUp]
        public void Setup()
        {
            _username = "username";
            _password = "password";
            Settings.Default.Username = _username;
            Settings.Default.Password = _password;

            _loginPageMock = new Mock<ILoginPage>();

            SiteNavigatorMock.Setup(navigator => navigator.GoToLoginPage()).Returns(_loginPageMock.Object);
        }

        [TearDown]
        public void Teardown()
        {
            Settings.Default.Reload();
        }

        [Test]
        public void ShouldReportNotLoggedInByDefault()
        {
            Assert.IsFalse(Manager.IsLoggedIn);

            SiteNavigatorMock.Verify(navigator => navigator.GoToLoginPage(), Times.Never());
        }

        [Test]
        public void ShouldNavigateToLoginPageWhenLoggingIn()
        {
            Manager.LogIn();

            _loginPageMock.Verify(page => page.LogIn(_username, _password));
            Assert.IsTrue(Manager.IsLoggedIn);
        }

        [Test]
        public void ShouldNotLogInIfAlreadyLoggedIn()
        {
            Manager.LogIn();

            Manager.EnsureLoggedIn();

            SiteNavigatorMock.Verify(nav => nav.GoToLoginPage(), Times.Once());
        }

        [Test]
        public void ShouldLogInIfNOtAlreadyLoggedIn()
        {
            Manager.EnsureLoggedIn();

            SiteNavigatorMock.Verify(nav => nav.GoToLoginPage(), Times.Once());
            Assert.IsTrue(Manager.IsLoggedIn);
        }
    }
}