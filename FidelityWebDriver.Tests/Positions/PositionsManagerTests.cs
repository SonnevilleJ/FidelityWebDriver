using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Login;
using Sonneville.FidelityWebDriver.Navigation;
using Sonneville.FidelityWebDriver.Positions;
using Sonneville.FidelityWebDriver.Tests.Navigation;

namespace Sonneville.FidelityWebDriver.Tests.Positions
{
    [TestFixture]
    public class PositionsManagerTests : ManagerTestsBase<IPositionsManager>
    {
        private Mock<ILoginManager> _loginManagerMock;
        private Mock<ISummaryPage> _summaryPageMock;
        private Mock<IPositionsPage> _positionsPage;
        private List<IAccount> _accountsList;

        protected override IPositionsManager InstantiateManager(ISiteNavigator siteNavigator)
        {
            _accountsList = new List<IAccount>();

            _positionsPage = new Mock<IPositionsPage>();
            _positionsPage.Setup(positionsPage => positionsPage.BuildAccounts()).Returns(_accountsList);

            _summaryPageMock = new Mock<ISummaryPage>();
            _summaryPageMock.Setup(summaryPage => summaryPage.GoToPositionsPage()).Returns(_positionsPage.Object);

            _loginManagerMock = new Mock<ILoginManager>();
            _loginManagerMock.Setup(manager => manager.EnsureLoggedIn()).Returns(_summaryPageMock.Object);

            return new PositionsManager(siteNavigator, _loginManagerMock.Object);
        }

        [Test]
        public void ShouldReturnAccountsFromPositionsPage()
        {
            var accounts = Manager.GetAccounts();

            Assert.AreSame(_accountsList, accounts);
        }
    }
}