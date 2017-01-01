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
                .Where(row => row.GetAttribute("class").Contains("normal-row") || row.GetAttribute("class").Contains("content-row"))
                .Select((row, index) => new KeyValuePair<int, IWebElement>(index, row))
                .GroupBy(kvp => kvp.Key / 2, kvp => kvp.Value, (i, elements) => elements)
                .Select(transactionRows => ParseTransactionFromRows(transactionRows.ToArray()));
        }

        private IFidelityTransaction ParseTransactionFromRows(IReadOnlyList<IWebElement> normalAndContentRows)
        {
            var result = new FidelityTransaction();
            normalAndContentRows[0].Click();
            var normalTDs = normalAndContentRows[0].FindElements(By.TagName("td"));
            result.RunDate = ParseDate(normalTDs[0].Text);
            result.AccountName = ParseAccountName(normalTDs[1]);
            result.AccountNumber = ParseAccountNumber(normalTDs[1]);
            result.SecurityDescription = ParseSecurityDescription(normalTDs[2]);
            result.Type = ParseType(result.SecurityDescription);

            var contentBody = normalAndContentRows[1].FindElement(By.TagName("tbody"));
            var tHeaders = contentBody.FindElements(By.TagName("th"));
            var tDatas = contentBody.FindElements(By.TagName("td"));

            var contentDictionary = tHeaders.Zip(tDatas, (th, td) => new KeyValuePair<string, string>(th.Text, td.Text))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            result.Amount = ParseCurrency(contentDictionary["Amount"]);
            switch (result.Type)
            {
                case TransactionType.Deposit:
                case TransactionType.DepositBrokeragelink:
                case TransactionType.DepositHSA:
                case TransactionType.Withdrawal:
                    break;
                case TransactionType.Buy:
                    result.Symbol = contentDictionary["Symbol"];
                    result.Quantity = ParseQuantity(contentDictionary["Shares"]);
                    result.Price = ParseDecimal(contentDictionary["Price"]);
                    result.SettlementDate = ParseDate(contentDictionary["Settlement Date"]);
                    break;
                case TransactionType.Sell:
                    result.Symbol = contentDictionary["Symbol"];
                    result.Quantity = ParseQuantity(contentDictionary["Shares"]);
                    result.Price = ParseDecimal(contentDictionary["Price"]);
                    result.Commission = ParseCurrency(contentDictionary["Commission"]);
                    result.SettlementDate = ParseDate(contentDictionary["Settlement Date"]);
                    break;
                case TransactionType.DividendReceipt:
                case TransactionType.ShortTermCapGain:
                case TransactionType.LongTermCapGain:
                    result.Symbol = contentDictionary["Symbol"];
                    break;
                case TransactionType.DividendReinvestment:
                    result.Symbol = contentDictionary["Symbol"];
                    result.Quantity = ParseQuantity(contentDictionary["Shares"]);
                    result.Price = ParseDecimal(contentDictionary["Price"]);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }

        private decimal ParseCurrency(string text)
        {
            return decimal.Parse(text, NumberStyles.Currency);
        }

        private decimal ParseQuantity(string text)
        {
            return decimal.Parse(text.Replace("+ ", ""));
        }

        private decimal ParseDecimal(string text)
        {
            return decimal.Parse(text);
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

        private DateTime ParseDate(string text)
        {
            return DateTime.ParseExact(text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
