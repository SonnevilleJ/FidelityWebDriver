using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Tests.Transactions
{
    public class HistoryHtmlGenerator
    {
        public IEnumerable<IWebElement> MapToTableRows(IFidelityTransaction transaction)
        {
            yield return CreateNormalRow(transaction);
            yield return CreateDepositContentRow(transaction);
        }

        private IWebElement CreateNormalRow(IFidelityTransaction transaction)
        {
            var trNormalRowExpandableMock = CreateNormalRowMock();
            trNormalRowExpandableMock.Setup(tr => tr.FindElements(By.TagName("td")))
                .Returns(new List<IWebElement>
                {
                    CreateDateTd(transaction.SettlementDate),
                    CreateAccountTd(transaction.AccountName, transaction.AccountNumber),
                    CreateDescriptionTd(transaction.SecurityDescription),
                }.AsReadOnly);
            return trNormalRowExpandableMock.Object;
        }

        private IWebElement CreateDepositContentRow(IFidelityTransaction transaction)
        {
            var trContentRowMock = CreateContentRowMock();
            trContentRowMock.Setup(tr => tr.FindElements(By.TagName("tr")))
                .Returns(CreateActivityTrs(transaction).ToList().AsReadOnly());
            return trContentRowMock.Object;
        }

        private IEnumerable<IWebElement> CreateActivityTrs(IFidelityTransaction transaction)
        {
            var trs = CreateTrDictionary(transaction)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var trMock = new Mock<IWebElement>();
            trMock.Setup(tr => tr.FindElements(By.TagName("th")))
                .Returns(trs.Keys.ToList().AsReadOnly);
            trMock.Setup(tr => tr.FindElements(By.TagName("td")))
                .Returns(trs.Values.ToList().AsReadOnly);
            yield return trMock.Object;
        }

        private IEnumerable<KeyValuePair<IWebElement, IWebElement>> CreateTrDictionary(IFidelityTransaction transaction)
        {
            switch (transaction.Type)
            {
                case TransactionType.Unknown:
                    throw new NotImplementedException();
                case TransactionType.Deposit:
                case TransactionType.DepositBrokeragelink:
                case TransactionType.DepositHSA:
                    yield return CreateActivityKeyValuePair("Amount", transaction.Amount?.ToString("C"));
                    break;
                case TransactionType.Withdrawal:
                    yield return CreateActivityKeyValuePair("Amount", transaction.Amount?.ToString("C"));
                    break;
                case TransactionType.Buy:
                    break;
                case TransactionType.Sell:
                    break;
                case TransactionType.DividendReceipt:
                    yield return CreateActivityKeyValuePair("Symbol", transaction.Symbol);
                    yield return CreateActivityKeyValuePair("Description", transaction.SecurityDescription);
                    yield return CreateActivityKeyValuePair("Amount", transaction.Amount?.ToString("C"));
                    break;
                case TransactionType.ShortTermCapGain:
                    break;
                case TransactionType.LongTermCapGain:
                    break;
                case TransactionType.DividendReinvestment:
                    yield return CreateActivityKeyValuePair("Symbol", transaction.Symbol);
                    yield return CreateActivityKeyValuePair("Description", transaction.SecurityDescription);
                    yield return CreateActivityKeyValuePair("Shares", transaction.Quantity?.ToString("+ 0.###"));
                    yield return CreateActivityKeyValuePair("Price", transaction.Price?.ToString("F"));
                    yield return CreateActivityKeyValuePair("Amount", transaction.Amount?.ToString("C"));
                    break;
                case TransactionType.SellShort:
                    break;
                case TransactionType.BuyToCover:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private KeyValuePair<IWebElement, IWebElement> CreateActivityKeyValuePair(string label, string value)
        {
            return new KeyValuePair<IWebElement, IWebElement>(CreateActivityThMock(label).Object, CreateActivityTdMock(value).Object);
        }

        private Mock<IWebElement> CreateNormalRowMock()
        {
            var trMock = new Mock<IWebElement>();
            trMock.Setup(tr => tr.GetAttribute("class")).Returns("normal-row expandable");
            return trMock;
        }

        private Mock<IWebElement> CreateContentRowMock()
        {
            var trMock = new Mock<IWebElement>();
            trMock.Setup(tr => tr.GetAttribute("class")).Returns("content-row");
            return trMock;
        }

        private IWebElement CreateDateTd(DateTime? settlementDate)
        {
            var dateTd = new Mock<IWebElement>();
            dateTd.Setup(td => td.Text).Returns(settlementDate?.ToString("MM/dd/yyyy"));
            return dateTd.Object;
        }

        private IWebElement CreateAccountTd(string accountName, string accountNumber)
        {
            var accountTd = new Mock<IWebElement>();
            accountTd.Setup(td => td.FindElements(By.TagName("span")))
                .Returns(new List<IWebElement>
                {
                    CreateSpanMock(accountName).Object,
                    CreateSpanMock(accountNumber).Object
                }.AsReadOnly());
            return accountTd.Object;
        }

        private static Mock<IWebElement> CreateSpanMock(string text)
        {
            var accountNumberSpan = new Mock<IWebElement>();
            accountNumberSpan.Setup(span => span.Text).Returns(text);
            return accountNumberSpan;
        }

        private IWebElement CreateDescriptionTd(string descriptionText)
        {
            var tdMock = new Mock<IWebElement>();
            tdMock.Setup(td => td.Text).Returns(descriptionText);
            return tdMock.Object;
        }

        private Mock<IWebElement> CreateActivityThMock(string label)
        {
            var thMock = new Mock<IWebElement>();
            thMock.Setup(th => th.Text).Returns(label);
            return thMock;
        }

        private Mock<IWebElement> CreateActivityTdMock(string value)
        {
            var tdMock = new Mock<IWebElement>();
            tdMock.Setup(td => td.Text).Returns(value);
            return tdMock;
        }
    }
}