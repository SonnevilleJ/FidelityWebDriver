using System.IO.IsolatedStorage;
using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Login;
using Sonneville.FidelityWebDriver.Navigation;
using Sonneville.Utilities.Configuration;

namespace Sonneville.FidelityWebDriver.Tests.Login
{
    [TestFixture]
    public class LoginManagerTests : ManagerTestsBase<LoginManager>
    {
        private Mock<ILoginPage> _loginPageMock;
        private FidelityConfiguration _fidelityConfiguration;
        private Mock<ISummaryPage> _summaryPageMock;

        protected override LoginManager InstantiateManager(ISiteNavigator siteNavigator)
        {
            _fidelityConfiguration = new ConfigStore(IsolatedStorageFile.GetUserStoreForAssembly()).Get<FidelityConfiguration>();
            _fidelityConfiguration.Username = "username";
            _fidelityConfiguration.Password = "password";

            return new LoginManager(siteNavigator, _fidelityConfiguration);
        }

        [SetUp]
        public void Setup()
        {
            _summaryPageMock = new Mock<ISummaryPage>();
            SiteNavigatorMock.Setup(navigator => navigator.GoTo<ISummaryPage>()).Returns(_summaryPageMock.Object);

            _loginPageMock = new Mock<ILoginPage>();
            _loginPageMock.Setup(
                loginPage => loginPage.LogIn(_fidelityConfiguration.Username, _fidelityConfiguration.Password))
                .Returns(_summaryPageMock.Object);

            SiteNavigatorMock.Setup(navigator => navigator.GoTo<ILoginPage>()).Returns(_loginPageMock.Object);
        }

        [Test]
        public void ShouldReportNotLoggedInByDefault()
        {
            Assert.IsFalse(Manager.IsLoggedIn);

            SiteNavigatorMock.Verify(navigator => navigator.GoTo<ILoginPage>(), Times.Never());
        }

        [Test]
        public void ShouldNavigateToSummaryPageAfterSuccessfulLogIn()
        {
            var summaryPage = Manager.EnsureLoggedIn();

            _loginPageMock.Verify(page => page.LogIn(_fidelityConfiguration.Username, _fidelityConfiguration.Password));
            Assert.IsTrue(Manager.IsLoggedIn);
            Assert.AreSame(_summaryPageMock.Object, summaryPage);
        }

        [Test]
        public void ShouldNotLogInIfAlreadyLoggedIn()
        {
            Manager.EnsureLoggedIn();

            var summaryPage = Manager.EnsureLoggedIn();

            SiteNavigatorMock.Verify(nav => nav.GoTo<ILoginPage>(), Times.Once());
            Assert.AreSame(_summaryPageMock.Object, summaryPage);
        }

        [Test]
        public void ShouldLogInIfNOtAlreadyLoggedIn()
        {
            var summaryPage = Manager.EnsureLoggedIn();

            SiteNavigatorMock.Verify(nav => nav.GoTo<ILoginPage>(), Times.Once());
            Assert.IsTrue(Manager.IsLoggedIn);
            Assert.AreSame(_summaryPageMock.Object, summaryPage);
        }
    }
}