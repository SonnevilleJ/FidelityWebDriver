using System;
using System.Collections.Generic;
using Moq;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Transactions.CSV;

namespace Sonneville.FidelityWebDriver.Tests.Transactions
{
    public class HistoryHtmlGenerator
    {
        private readonly TransactionTypeMapper _transactionTypeMapper = new TransactionTypeMapper();

        public IEnumerable<IWebElement> MapToContentRow(IFidelityTransaction transaction)
        {
            switch (transaction.Type)
            {
                case TransactionType.Unknown:
                    throw new NotImplementedException();
                case TransactionType.Deposit:
                    break;
                case TransactionType.DepositBrokeragelink:
                    var trNormalRowExpandableMock = CreateNormalRowMock();
                    trNormalRowExpandableMock.Setup(tr => tr.FindElements(By.TagName("td")))
                        .Returns(new List<IWebElement>
                        {
                            CreateDateTd(transaction.SettlementDate),
                            CreateAccountTd(transaction.AccountName, transaction.AccountNumber),
                            CreateDescriptionTd(_transactionTypeMapper.MapKey(transaction.Type)),
                        }.AsReadOnly);
                    yield return trNormalRowExpandableMock.Object;
                    var trContentRowMock = CreateContentRowMock();
                    trContentRowMock.Setup(tr => tr.FindElements(By.TagName("tr")))
                        .Returns(new List<IWebElement>
                        {
                            CreateActivityAmountTr(transaction.Amount),
                        }.AsReadOnly);
                    yield return trContentRowMock.Object;
                    break;
                case TransactionType.DepositHSA:
                    break;
                case TransactionType.Withdrawal:
                    break;
                case TransactionType.Buy:
                    break;
                case TransactionType.Sell:
                    break;
                case TransactionType.DividendReceipt:
                    break;
                case TransactionType.ShortTermCapGain:
                    break;
                case TransactionType.LongTermCapGain:
                    break;
                case TransactionType.DividendReinvestment:
                    break;
                case TransactionType.SellShort:
                    break;
                case TransactionType.BuyToCover:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        private IWebElement CreateActivityAmountTr(decimal? amount)
        {
            var trMock = new Mock<IWebElement>();
            trMock.Setup(tr => tr.FindElements(By.TagName("th")))
                .Returns(new List<IWebElement>
                {
                    CreateActivityThMock("Amount").Object,
                }.AsReadOnly);
            trMock.Setup(tr => tr.FindElements(By.TagName("td")))
                .Returns(new List<IWebElement>
                {
                    CreateActivityTdMock(amount?.ToString("C")).Object,
                }.AsReadOnly);

            return trMock.Object;
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