using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
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
        private List<AccountSummary> _accountSummaries;

        private Mock<ITransactionManager> _transactionManagerMock;
        private List<IFidelityTransaction> _transactions;
        private List<AccountDetails> _accountDetails;
        private IsolatedStorageFile _isolatedStore;

        [SetUp]
        public void Setup()
        {
            _cliUserName = "Batman";
            _cliPassword = "I am vengeance. I am the night. I am Batman.";

            _accountSummaries = new List<AccountSummary>
            {
                new AccountSummary
                {
                    AccountNumber = "acct 1",
                    AccountType = AccountType.InvestmentAccount,
                    Name = "play money",
                    MostRecentValue = 5000
                },
                new AccountSummary
                {
                    AccountNumber = "acct 2",
                    AccountType = AccountType.RetirementAccount,
                    Name = "play money",
                    MostRecentValue = 88176
                },
                new AccountSummary
                {
                    AccountNumber = "acct 3",
                    AccountType = AccountType.HealthSavingsAccount,
                    Name = "don't get sick",
                    MostRecentValue = 1800
                },
                new AccountSummary
                {
                    AccountNumber = "acct 4",
                    AccountType = AccountType.CreditCard,
                    Name = "debt",
                    MostRecentValue = 1200
                },
            };

            _accountDetails = new List<AccountDetails>()
            {
                new AccountDetails
                {
                    Name = "first account",
                    AccountNumber = "acct a",
                    Positions = new List<IPosition>
                    {
                        new Position
                        {
                            Ticker = "asdf",
                            Quantity = 2,
                            CostBasisPerShare = 5.27m,
                            CurrentValue = 10.54m,
                        },
                        new Position
                        {
                            Ticker = "querty",
                            Quantity = 35,
                            CostBasisPerShare = 8.49m,
                            CurrentValue = 297.15m,
                        },
                    }
                },
                new AccountDetails
                {
                    Name = "second account",
                    AccountNumber = "acct b",
                    Positions = new List<IPosition>
                    {
                        new Position
                        {
                            Ticker = "aapl",
                            Quantity = 2,
                            CostBasisPerShare = 195.27m,
                            CurrentValue = 390.54m,
                        },
                        new Position
                        {
                            Ticker = "msft",
                            Quantity = 35,
                            CostBasisPerShare = 1.04m,
                            CurrentValue = 36.4m,
                        },
                    }
                },
            };

            _transactions = new List<IFidelityTransaction>
            {
                new FidelityTransaction
                {
                    RunDate = new DateTime(2015, 12, 25),
                    Account = null,
                    Action = null,
                    Type = TransactionType.Buy,
                    Symbol = "DE",
                    SecurityDescription = null,
                    SecurityType = null,
                    Quantity = 12.3m,
                    Price = 45.67m,
                    Commission = 8.9m,
                    Fees = 0.0m,
                    AccruedInterest = 0.0m,
                    Amount = 0.0m,
                    SettlementDate = new DateTime(2015, 12, 31),
                }
            };

            _positionsManagerMock = new Mock<IPositionsManager>();
            _positionsManagerMock.Setup(manager => manager.GetAccountSummaries()).Returns(_accountSummaries);
            _positionsManagerMock.Setup(manager => manager.GetAccountDetails()).Returns(_accountDetails);

            _transactionManagerMock = new Mock<ITransactionManager>();
            _transactionManagerMock.Setup(manager => manager.DownloadTransactionHistory()).Returns(_transactions);

            _isolatedStore = IsolatedStorageFile.GetUserStoreForAssembly();
            _fidelityConfiguration = FidelityConfiguration.Initialize(_isolatedStore);

            _app = new App(_positionsManagerMock.Object, _transactionManagerMock.Object, _fidelityConfiguration,
                new TransactionTranslator());
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
        public void ShouldFetchAccountSummariesFromPositionsManager()
        {
            using (var memoryStream = new MemoryStream())
            {
                RedirectConsoleOutput(memoryStream);

                _app.Run(new string[0]);

                var consoleOutput = ReadConsoleOutput(memoryStream);
                _accountSummaries.ForEach(account =>
                {
                    Assert.IsTrue(consoleOutput.Contains(account.AccountNumber));
                    Assert.IsTrue(consoleOutput.Contains(account.Name));
                    Assert.IsTrue(consoleOutput.Contains(account.MostRecentValue.ToString("C")));
                });
            }
        }

        [Test]
        public void ShouldFetchAccountDetailsFromPositionsManager()
        {
            using (var memoryStream = new MemoryStream())
            {
                RedirectConsoleOutput(memoryStream);

                _app.Run(new string[0]);

                var consoleOutput = ReadConsoleOutput(memoryStream);
                _accountDetails.ForEach(account =>
                {
                    Assert.IsTrue(consoleOutput.Contains(account.Name));
                    Assert.IsTrue(consoleOutput.Contains(account.AccountNumber));
                    Assert.IsTrue(consoleOutput.Contains(account.AccountType.ToString()));
                    account.Positions.ToList().ForEach(position =>
                    {
                        Assert.IsTrue(consoleOutput.Contains(position.Ticker));
                        Assert.IsTrue(consoleOutput.Contains(position.Quantity.ToString("N")));
                        Assert.IsTrue(consoleOutput.Contains(position.CurrentValue.ToString("C")));
                        Assert.IsTrue(consoleOutput.Contains(position.CostBasisPerShare.ToString("C")));
                    });
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

            var fidelityConfiguration = FidelityConfiguration.Initialize(_isolatedStore);
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
                _app?.Dispose();
                _app = null;
            }
        }
    }
}