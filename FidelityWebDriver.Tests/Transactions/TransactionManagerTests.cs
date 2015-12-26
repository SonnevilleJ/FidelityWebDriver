using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Login;
using Sonneville.FidelityWebDriver.Navigation;
using Sonneville.FidelityWebDriver.Tests.Navigation;
using Sonneville.FidelityWebDriver.Transactions;
using Sonneville.FidelityWebDriver.Transactions.CSV;

namespace Sonneville.FidelityWebDriver.Tests.Transactions
{
    [TestFixture]
    public class TransactionManagerTests : ManagerTestsBase<TransactionManager>
    {
        private Mock<ILoginManager> _loginManagerMock;
        private Mock<IActivityPage> _activityPageMock;
        private Mock<ITransactionsMapper> _csvParserMock;
        private List<IFidelityTransaction> _transactions;

        protected override TransactionManager InstantiateManager(ISiteNavigator siteNavigator)
        {
            var downloadPath = "file path";
            _transactions = new List<IFidelityTransaction>();

            _activityPageMock = new Mock<IActivityPage>();
            _activityPageMock.Setup(
                activityPage => activityPage.DownloadHistory(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(downloadPath);

            SiteNavigatorMock.Setup(navigator => navigator.GoTo<IActivityPage>())
                .Returns(_activityPageMock.Object);

            _loginManagerMock = new Mock<ILoginManager>();

            _csvParserMock = new Mock<ITransactionsMapper>();
            _csvParserMock.Setup(parser => parser.ParseCsv(downloadPath)).Returns(_transactions);

            return new TransactionManager(siteNavigator, _loginManagerMock.Object, _csvParserMock.Object);
        }

        [Test]
        public void ShouldReturnParsedTransactions()
        {
            var actualTransactions = Manager.DownloadTransactionHistory();

            _loginManagerMock.Verify(manager => manager.EnsureLoggedIn());
            SiteNavigatorMock.Verify(navigator => navigator.GoTo<IActivityPage>());
            Assert.AreEqual(_transactions, actualTransactions);
        }
    }
}