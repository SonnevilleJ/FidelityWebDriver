using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Positions;
using Sonneville.FidelityWebDriver.Transactions;

namespace Sonneville.FidelityWebDriver.Demo.Tests
{
    [TestFixture]
    public class AppTests : IDisposable
    {
        private App _app;
        private Mock<IPositionsManager> _positionsManagerMock;
        private FidelityConfiguration _fidelityConfiguration;
        private string _cliUserName;
        private string _cliPassword;
        private List<AccountSummary> _accounts;

        private Mock<ITransactionManager> _transactionManagerMock;
        private List<IFidelityTransaction> _transactions;

        [SetUp]
        public void Setup()
        {
            _cliUserName = "Batman";
            _cliPassword = "I am vengeance. I am the night. I am Batman.";

            _accounts = new List<AccountSummary>
            {
                new AccountSummary("acct 1", AccountType.InvestmentAccount, "play money", 5000),
                new AccountSummary("acct 2", AccountType.RetirementAccount, "play money", 88176),
                new AccountSummary("acct 3", AccountType.HealthSavingsAccount, "don't get sick", 1800),
                new AccountSummary("acct 4", AccountType.CreditCard, "debt", 1200),
            };

            _transactions = new List<IFidelityTransaction>
            {
                new FidelityTransaction(new DateTime(2015, 12, 25), null, null, TransactionType.Buy, "DE", null, null, 12.3m, 45.67m, 8.9m, 0.0m, 0.0m, 0.0m, new DateTime(2015, 12, 31))
            };

            _positionsManagerMock = new Mock<IPositionsManager>();
            _positionsManagerMock.Setup(manager => manager.GetAccounts()).Returns(_accounts);

            _transactionManagerMock = new Mock<ITransactionManager>();
            _transactionManagerMock.Setup(manager => manager.DownloadTransactionHistory()).Returns(_transactions);

            _fidelityConfiguration = new FidelityConfiguration();
            _fidelityConfiguration.Initialize();

            _app = new App(_positionsManagerMock.Object, _transactionManagerMock.Object, _fidelityConfiguration);
        }

        [TearDown]
        public void Teardown()
        {
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true});

            _fidelityConfiguration.Username = null;
            _fidelityConfiguration.Password = null;
            _fidelityConfiguration.Write();
        }

        [Test]
        public void ShouldFetchAccountInfoFromPositionsManager()
        {
            using (var memoryStream = new MemoryStream())
            {
                RedirectConsoleOutput(memoryStream);
                
                _app.Run(new string[0]);

                var consoleOutput = ReadConsoleOutput(memoryStream);
                _accounts.ForEach(account =>
                {
                    Assert.IsTrue(consoleOutput.Contains(account.AccountNumber));
                    Assert.IsTrue(consoleOutput.Contains(account.Name));
                    Assert.IsTrue(consoleOutput.Contains(account.MostRecentValue.ToString("C")));
                });
            }
        }

        [Test]
        public void ShouldDownloadTransactionHistoryFromTransactionsManager()
        {
            using (var memoryStream = new MemoryStream())
            {
                RedirectConsoleOutput(memoryStream);

                _app.Run(new string[0]);

                var consoleOutput = ReadConsoleOutput(memoryStream);
                _transactions.ForEach(transaction =>
                {
                    Assert.IsTrue(consoleOutput.Contains(transaction.RunDate.Value.ToString("d")));
                    Assert.IsTrue(consoleOutput.Contains(transaction.Quantity.Value.ToString("F")));
                    Assert.IsTrue(consoleOutput.Contains(transaction.Symbol));
                    Assert.IsTrue(consoleOutput.Contains(transaction.Price.Value.ToString("C")));
                });
            }

            _transactionManagerMock.Verify(manager => manager.DownloadTransactionHistory());
        }

        [Test]
        public void ShouldSetConfigFromCliArgsWithoutPersisting()
        {
            var args = new[] {"-u", _cliUserName, "-p", _cliPassword};

            _app.Run(args);

            Assert.AreEqual(_cliUserName, _fidelityConfiguration.Username);
            Assert.AreEqual(_cliPassword, _fidelityConfiguration.Password);
            AssertUnchangedConfig();
        }

        [Test]
        public void ShouldSetConfigFromCliArgsAndPersist()
        {
            var args = new[] {"-u", _cliUserName, "-p", _cliPassword, "-s"};

            _app.Run(args);

            var fidelityConfiguration = new FidelityConfiguration();
            fidelityConfiguration.Initialize();
            Assert.AreEqual(_cliUserName, fidelityConfiguration.Username);
            Assert.AreEqual(_cliPassword, fidelityConfiguration.Password);
        }

        [Test]
        public void ShouldDisplayHelpFromCliArgsAndNotPersist()
        {
            using (var memoryStream = new MemoryStream())
            {
                RedirectConsoleOutput(memoryStream);

                _app.Run(new[] {"-u", _cliUserName, "-p", _cliPassword, "-s", "-h"});

                var consoleOutput = ReadConsoleOutput(memoryStream);
                Assert.IsTrue(consoleOutput.Contains("-h"),
                    $"Actual console output follows:{Environment.NewLine}{consoleOutput}");
                AssertUnchangedConfig();
            }
        }

        [Test]
        public void ShouldCascadeDisposeToManagers()
        {
            _app.Dispose();

            _positionsManagerMock.Verify(manager => manager.Dispose());
            _transactionManagerMock.Verify(manager => manager.Dispose());
        }

        [Test]
        public void ShouldHandleMultipleDisposals()
        {
            _app.Dispose();
            _app.Dispose();
        }

        private void AssertUnchangedConfig()
        {
            _fidelityConfiguration.Read();
            Assert.IsNull(_fidelityConfiguration.Username);
            Assert.IsNull(_fidelityConfiguration.Password);
        }

        private static void RedirectConsoleOutput(Stream memoryStream)
        {
            var streamWriter = new StreamWriter(memoryStream) {AutoFlush = true};
            Console.SetOut(streamWriter);
        }

        private static string ReadConsoleOutput(Stream memoryStream)
        {
            memoryStream.Position = 0;
            return new StreamReader(memoryStream).ReadToEnd();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var app = _app;
                if (app != null)
                {
                    app.Dispose();
                    _app = null;
                }
            }
        }
    }
}