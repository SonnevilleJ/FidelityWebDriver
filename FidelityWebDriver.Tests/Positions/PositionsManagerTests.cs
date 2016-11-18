using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Login;
using Sonneville.FidelityWebDriver.Navigation;
using Sonneville.FidelityWebDriver.Positions;

namespace Sonneville.FidelityWebDriver.Tests.Positions
{
    [TestFixture]
    public class PositionsManagerTests : ManagerTestsBase<IPositionsManager>
    {
        private Mock<ILoginManager> _loginManagerMock;
        private Mock<ISummaryPage> _summaryPageMock;
        private Mock<IPositionsPage> _positionsPage;
        private List<IAccountSummary> _accountSummaries;
        private List<IAccountDetails> _accountDetails;

        protected override IPositionsManager InstantiateManager(ISiteNavigator siteNavigator, FidelityConfiguration fidelityConfiguration)
        {
            _accountSummaries = new List<IAccountSummary>();
            _accountDetails = new List<IAccountDetails>();

            _positionsPage = new Mock<IPositionsPage>();
            _positionsPage.Setup(positionsPage => positionsPage.GetAccountSummaries())
                .Callback(()=>_loginManagerMock.Verify(loginManager=>loginManager.EnsureLoggedIn()))
                .Returns(_accountSummaries);
            _positionsPage.Setup(positionsPage => positionsPage.GetAccountDetails())
                .Callback(() => _loginManagerMock.Verify(loginManager => loginManager.EnsureLoggedIn()))
                .Returns(_accountDetails);

            _summaryPageMock = new Mock<ISummaryPage>();
            _summaryPageMock.Setup(summaryPage => summaryPage.GoToPositionsPage()).Returns(_positionsPage.Object);

            _loginManagerMock = new Mock<ILoginManager>();
            _loginManagerMock.Setup(manager => manager.EnsureLoggedIn()).Returns(_summaryPageMock.Object);

            return new PositionsManager(siteNavigator, _loginManagerMock.Object);
        }

        [SetUp]
        public void Setup()
        {
            base.SetupTestsBase();
        }

        [Test]
        public void ShouldDisposeLoginManager()
        {
            Manager.Dispose();

            _loginManagerMock.Verify(loginManager => loginManager.Dispose());
        }

        [Test]
        public void ShouldReturnAccountSummariesFromPositionsPage()
        {
            var accounts = Manager.GetAccountSummaries();

            Assert.AreSame(_accountSummaries, accounts);
        }

        [Test]
        public void ShouldReturnAccountDetailsFromPositionsPage()
        {
            var accounts = Manager.GetAccountDetails();

            Assert.AreSame(_accountDetails, accounts);
        }
    }
}