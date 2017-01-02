using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Login;
using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Tests.Login
{
    [TestFixture]
    public class LoginManagerTests : ManagerTestsBase<LoginManager>
    {
        private Mock<ILoginPage> _loginPageMock;
        private Mock<ISummaryPage> _summaryPageMock;

        protected override LoginManager InstantiateManager()
        {
            FidelityConfiguration.Username = "username";
            FidelityConfiguration.Password = "password";

            return new LoginManager(LogMock.Object, SiteNavigatorMock.Object, FidelityConfiguration);
        }

        [SetUp]
        public void Setup()
        {
            SetupTestsBase();
            _summaryPageMock = new Mock<ISummaryPage>();
            SiteNavigatorMock.Setup(navigator => navigator.GoTo<ISummaryPage>()).Returns(_summaryPageMock.Object);

            _loginPageMock = new Mock<ILoginPage>();

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

            _loginPageMock.Verify(page => page.LogIn(FidelityConfiguration.Username, FidelityConfiguration.Password));
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
        public void ShouldLogInIfNotAlreadyLoggedIn()
        {
            var summaryPage = Manager.EnsureLoggedIn();

            SiteNavigatorMock.Verify(nav => nav.GoTo<ILoginPage>(), Times.Once());
            Assert.IsTrue(Manager.IsLoggedIn);
            Assert.AreSame(_summaryPageMock.Object, summaryPage);
        }
    }
}