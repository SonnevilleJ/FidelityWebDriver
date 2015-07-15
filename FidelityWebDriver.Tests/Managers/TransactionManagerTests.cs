using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Managers;

namespace Sonneville.FidelityWebDriver.Tests.Managers
{
    [TestFixture]
    public class TransactionManagerTests : ManagerTestsBase<TransactionManager>
    {
        private Mock<ILoginManager> _loginManagerMock;

        protected override TransactionManager InstantiateManager(ISiteNavigator siteNavigator)
        {
            _loginManagerMock = new Mock<ILoginManager>();

            return new TransactionManager(siteNavigator, _loginManagerMock.Object);
        }

        [Test]
        public void ShouldPerformLoginIfNotLoggedIn()
        {
            Manager.DownloadTransactions();

            _loginManagerMock.Verify(manager => manager.EnsureLoggedIn());
        }
    }
}