using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Managers;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver.Tests.Managers
{
    [TestFixture]
    public class LoginManagerTests : ManagerTestsBase<LoginManager>
    {
        private Mock<ILoginPage> _loginPageMock;
        private FidelityConfiguration _fidelityConfiguration;
        private Mock<ISummaryPage> _summaryPageMock;

        protected override LoginManager InstantiateManager()
        {
            _fidelityConfiguration = new FidelityConfiguration
            {
                Username = "username",
                Password = "password"
            };

            return new LoginManager(SiteNavigatorMock.Object, _fidelityConfiguration);
        }

        [SetUp]
        public void Setup()
        {
            _summaryPageMock = new Mock<ISummaryPage>();

            _loginPageMock = new Mock<ILoginPage>();
            _loginPageMock.Setup(
                loginPage => loginPage.LogIn(_fidelityConfiguration.Username, _fidelityConfiguration.Password))
                .Returns(_summaryPageMock.Object);
            SiteNavigatorMock.Setup(navigator => navigator.GoToLoginPage()).Returns(_loginPageMock.Object);
        }

        [Test]
        public void ShouldReportNotLoggedInByDefault()
        {
            Assert.IsFalse(Manager.IsLoggedIn);

            SiteNavigatorMock.Verify(navigator => navigator.GoToLoginPage(), Times.Never());
        }

        [Test]
        public void ShouldNavigateToSummaryPageAfterSuccessfulLogIn()
        {
            var summaryPage = Manager.LogIn();

            _loginPageMock.Verify(page => page.LogIn(_fidelityConfiguration.Username, _fidelityConfiguration.Password));
            Assert.IsTrue(Manager.IsLoggedIn);
            Assert.AreSame(_summaryPageMock.Object, summaryPage);
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