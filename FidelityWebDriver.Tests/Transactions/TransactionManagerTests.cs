using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Login;
using Sonneville.FidelityWebDriver.Navigation;
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
        private DateTime _startDate;
        private DateTime _endDate;

        protected override TransactionManager InstantiateManager(
            ISiteNavigator siteNavigator,
            FidelityConfiguration fidelityConfiguration
        )
        {
            const string downloadPath = "file path";
            _startDate = DateTime.MinValue;
            _endDate = DateTime.Today;

            _transactions = new List<IFidelityTransaction>
            {
                new FidelityTransaction()
            };

            _activityPageMock = new Mock<IActivityPage>();
            _activityPageMock.Setup(
                    activityPage => activityPage.GetTransactions(_startDate, _endDate))
                .Returns(_transactions);

            SiteNavigatorMock.Setup(navigator => navigator.GoTo<IActivityPage>())
                .Returns(_activityPageMock.Object);

            _loginManagerMock = new Mock<ILoginManager>();

            _csvParserMock = new Mock<ITransactionsMapper>();
            _csvParserMock.Setup(parser => parser.ParseCsv(downloadPath)).Returns(_transactions);

            return new TransactionManager(siteNavigator, _loginManagerMock.Object);
        }

        [SetUp]
        public void Setup()
        {
            SetupTestsBase();
        }

        [Test]
        public void ShouldDisposeLoginManager()
        {
            Manager.Dispose();

            _loginManagerMock.Verify(loginManager => loginManager.Dispose());
        }

        [Test]
        public void ShouldReturnParsedTransactions()
        {
            var actualTransactions = Manager.DownloadTransactionHistory(_startDate, _endDate);

            _loginManagerMock.Verify(manager => manager.EnsureLoggedIn());
            SiteNavigatorMock.Verify(navigator => navigator.GoTo<IActivityPage>());
            CollectionAssert.AreEquivalent(_transactions, actualTransactions);
        }
    }
}