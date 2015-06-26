using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Managers;

namespace Sonneville.FidelityWebDriver.Tests.Managers
{
    [TestFixture]
    public class TransactionManagerTests : ManagerTestsBase<TransactionManager>
    {
        private Mock<ILoginManager> _loginManagerMock;

        protected override TransactionManager InstantiateManager()
        {
            _loginManagerMock = new Mock<ILoginManager>();

            return new TransactionManager(SiteNavigatorMock.Object, _loginManagerMock.Object);
        }

        [Test]
        public void ShouldPerformLoginIfNotLoggedIn()
        {
            Manager.DownloadTransactions();

            _loginManagerMock.Verify(manager => manager.EnsureLoggedIn());
        }

        [Test]
        public void ShouldDisposeSiteNavigatorWhenRun()
        {
            Manager.DownloadTransactions();

            SiteNavigatorMock.Verify(navigator => navigator.Dispose());
        }

        [Test]
        public void ShouldObserveAutoCloseSettingWhenRun()
        {
            try
            {
                Settings.Default.AutoCloseSelenium = false;

                Manager.DownloadTransactions();

                SiteNavigatorMock.Verify(navigator => navigator.Dispose(), Times.Never());
            }
            finally
            {
                Settings.Default.Reset();
            }
        }
    }
}