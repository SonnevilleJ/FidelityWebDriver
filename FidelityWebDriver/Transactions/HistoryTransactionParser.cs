using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Transactions.CSV;

namespace Sonneville.FidelityWebDriver.Transactions
{
    public class HistoryTransactionParser : IHistoryTransactionParser
    {
        private readonly ITransactionTypeMapper _transactionTypeMapper;

        public HistoryTransactionParser(ITransactionTypeMapper transactionTypeMapper)
        {
            _transactionTypeMapper = transactionTypeMapper;
        }

        public IEnumerable<IFidelityTransaction> ParseFidelityTransactions(IWebElement historyRoot)
        {
            return historyRoot.FindElements(By.TagName("tbody"))[0]
                .FindElements(By.TagName("tr"))
                .Select((row, index) => new KeyValuePair<int, IWebElement>(index, row))
                .GroupBy(kvp => kvp.Key / 2, kvp => kvp.Value, (i, elements) => elements)
                .Select(transactionRows => ParseTransactionFromRows(transactionRows.ToArray()));
        }

        private IFidelityTransaction ParseTransactionFromRows(IReadOnlyList<IWebElement> normalAndContentRows)
        {
            var result = new FidelityTransaction();
            var normalTDs = normalAndContentRows[0].FindElements(By.TagName("td"));
            result.SettlementDate = ParseSettlementDate(normalTDs[0]);
            result.AccountName = ParseAccountName(normalTDs[1]);
            result.AccountNumber = ParseAccountNumber(normalTDs[1]);
            result.SecurityDescription = ParseSecurityDescription(normalTDs[2]);
            result.Type = ParseType(result.SecurityDescription);

            var activityTr = normalAndContentRows[1].FindElements(By.TagName("tr"))[0];
            var tHeaders = activityTr.FindElements(By.TagName("th"));
            var tDatas = activityTr.FindElements(By.TagName("td"));

            var contentDictionary = tHeaders.Zip(tDatas, (th, td) => new KeyValuePair<string, string>(th.Text, td.Text))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            result.Amount = ParseAmount(contentDictionary["Amount"]);
            switch (result.Type)
            {
                case TransactionType.Unknown:
                    break;
                case TransactionType.Deposit:
                    break;
                case TransactionType.DepositBrokeragelink:
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
                    result.Symbol = contentDictionary["Symbol"];
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

            return result;
        }

        private decimal ParseAmount(string amountText)
        {
            return decimal.Parse(amountText, NumberStyles.Currency);
        }

        private TransactionType ParseType(string description)
        {
            return _transactionTypeMapper.MapValue(description);
        }

        private string ParseSecurityDescription(IWebElement descriptionTd)
        {
            return descriptionTd.Text;
        }

        private string ParseAccountNumber(IWebElement accountTd)
        {
            var accountNumberSpan = accountTd.FindElements(By.TagName("span"))[1];
            return accountNumberSpan.Text;
        }

        private string ParseAccountName(IWebElement accountTd)
        {
            var accountNameSpan = accountTd.FindElements(By.TagName("span"))[0];
            return accountNameSpan.Text;
        }

        private DateTime ParseSettlementDate(IWebElement settlementDateTd)
        {
            return DateTime.ParseExact(settlementDateTd.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        }
    }
}