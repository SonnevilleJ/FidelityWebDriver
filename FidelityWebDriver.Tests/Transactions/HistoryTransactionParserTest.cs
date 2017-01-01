using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Transactions;
using Sonneville.FidelityWebDriver.Transactions.CSV;

namespace Sonneville.FidelityWebDriver.Tests.Transactions
{
    [TestFixture]
    public class HistoryTransactionParserTest
    {
        private readonly HistoryHtmlGenerator _historyHtmlGenerator = new HistoryHtmlGenerator();
        private readonly TransactionTypeMapper _transactionTypeMapper = new TransactionTypeMapper();

        private Mock<IWebElement> _historyRootDivMock;
        private Mock<IWebElement> _historyTableBodyMock;

        private HistoryTransactionParser _historyTransactionParser;

        [SetUp]
        public void Setup()
        {
            _historyTableBodyMock = new Mock<IWebElement>();

            _historyRootDivMock = new Mock<IWebElement>();
            _historyRootDivMock.Setup(div => div.FindElements(By.TagName("tbody")))
                .Returns(new List<IWebElement>
                {
                    _historyTableBodyMock.Object,
                    new Mock<IWebElement>().Object,
                }.AsReadOnly);

            _historyTransactionParser = new HistoryTransactionParser(_transactionTypeMapper);
        }

        [Test]
        public void GetHistoryShouldParseDepositTransactions()
        {
            var expectedTransactions = new List<IFidelityTransaction>
            {
                CreateDepositTransaction()
            };
            SetupHistoryTable(expectedTransactions);

            var actualTransactions = _historyTransactionParser.ParseFidelityTransactions(_historyRootDivMock.Object);

            CollectionAssert.AreEquivalent(expectedTransactions, actualTransactions);
        }

        [Test]
        public void GetHistoryShouldParseDepositBrokerageLinkTransactions()
        {
            var expectedTransactions = new List<IFidelityTransaction>
            {
                CreateDepositBrokeragelinkTransaction()
            };
            SetupHistoryTable(expectedTransactions);

            var actualTransactions = _historyTransactionParser.ParseFidelityTransactions(_historyRootDivMock.Object);

            CollectionAssert.AreEquivalent(expectedTransactions, actualTransactions);
        }

        [Test]
        public void GetHistoryShouldParseDepositHsaTransactions()
        {
            var expectedTransactions = new List<IFidelityTransaction>
            {
                CreateDepositHsaTransaction()
            };
            SetupHistoryTable(expectedTransactions);

            var actualTransactions = _historyTransactionParser.ParseFidelityTransactions(_historyRootDivMock.Object);

            CollectionAssert.AreEquivalent(expectedTransactions, actualTransactions);
        }

        [Test]
        public void GetHistoryShouldParseDividendReceiptTransactions()
        {
            var expectedTransactions = new List<IFidelityTransaction>
            {
                CreateDividendReceivedTransaction()
            };
            SetupHistoryTable(expectedTransactions);

            var actualTransactions = _historyTransactionParser.ParseFidelityTransactions(_historyRootDivMock.Object);

            CollectionAssert.AreEquivalent(expectedTransactions, actualTransactions);
        }

        private void SetupHistoryTable(List<IFidelityTransaction> expectedTransactions)
        {
            _historyTableBodyMock.Setup(div => div.FindElements(By.TagName("tr")))
                .Returns(expectedTransactions
                    .SelectMany(_historyHtmlGenerator.MapToTableRows)
                    .ToList()
                    .AsReadOnly());
        }

        private IFidelityTransaction CreateDepositTransaction(TransactionType transactionType = TransactionType.Deposit)
        {
            return new FidelityTransaction
            {
                Type = transactionType,
                SecurityDescription = _transactionTypeMapper.MapKey(transactionType),
                AccountName = "account name",
                AccountNumber = "account number",
                Amount = 1234.50m,
                SettlementDate = new DateTime(2016, 12, 26),
            };
        }

        private IFidelityTransaction CreateDepositBrokeragelinkTransaction()
        {
            return CreateDepositTransaction(TransactionType.DepositBrokeragelink);
        }

        private IFidelityTransaction CreateDepositHsaTransaction()
        {
            return CreateDepositTransaction(TransactionType.DepositHSA);
        }

        private IFidelityTransaction CreateDividendReceivedTransaction()
        {
            return new FidelityTransaction
            {
                Type = TransactionType.DividendReceipt,
                Symbol = "ASDF",
                SecurityDescription = _transactionTypeMapper.MapKey(TransactionType.DividendReceipt),
                AccountName = "account name",
                AccountNumber = "account number",
                Amount = 1234.50m,
                SettlementDate = new DateTime(2016, 12, 26),
            };
        }
    }
}