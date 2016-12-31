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
        private List<IFidelityTransaction> _expectedTransactions;

        [SetUp]
        public void Setup()
        {
            _expectedTransactions = new List<IFidelityTransaction>
            {
                CreateDepositBrokeragelinkTransaction(),
            };

            _historyTableBodyMock = new Mock<IWebElement>();

            _historyTableBodyMock.Setup(div => div.FindElements(By.TagName("tr")))
                .Returns(_expectedTransactions
                    .SelectMany(_historyHtmlGenerator.MapToContentRow)
                    .ToList()
                    .AsReadOnly());

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
        public void GetHistoryShouldReturnTransactions()
        {
            var actualTransactions = _historyTransactionParser.ParseFidelityTransactions(_historyRootDivMock.Object);

            CollectionAssert.AreEquivalent(_expectedTransactions, actualTransactions);
        }

        private IFidelityTransaction CreateDepositBrokeragelinkTransaction()
        {
            return new FidelityTransaction
            {
                Type = TransactionType.DepositBrokeragelink,
                SecurityDescription = _transactionTypeMapper.MapKey(TransactionType.DepositBrokeragelink),
                AccountName = "account name",
                AccountNumber = "account number",
                Amount = 1234.50m,
                SettlementDate = new DateTime(2016, 12, 26),
            };
        }
    }
}