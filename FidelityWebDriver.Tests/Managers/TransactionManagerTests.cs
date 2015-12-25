using System;
using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Managers;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver.Tests.Managers
{
    [TestFixture]
    public class TransactionManagerTests : ManagerTestsBase<TransactionManager>
    {
        private Mock<ILoginManager> _loginManagerMock;

        private Mock<IActivityPage> _activityPageMock;

        protected override TransactionManager InstantiateManager(ISiteNavigator siteNavigator)
        {
            _activityPageMock = new Mock<IActivityPage>();
            SiteNavigatorMock.Setup(navigator => navigator.GoTo<IActivityPage>())
                .Returns(_activityPageMock.Object);

            _loginManagerMock = new Mock<ILoginManager>();

            return new TransactionManager(siteNavigator, _loginManagerMock.Object);
        }

        [Test]
        public void ShouldPerformLoginIfNotLoggedIn()
        {
            Manager.DownloadTransactionHistory();

            _loginManagerMock.Verify(manager => manager.EnsureLoggedIn());
            SiteNavigatorMock.Verify(navigator=>navigator.GoTo<IActivityPage>());
            _activityPageMock.Verify(
                activityPage => activityPage.DownloadHistory(It.IsAny<DateTime>(), It.IsAny<DateTime>()));
        }
    }
}