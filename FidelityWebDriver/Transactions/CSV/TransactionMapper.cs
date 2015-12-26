using System;
using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Transactions.CSV
{
    public class TransactionMapper : ITransactionMapper
    {
        private readonly ITransactionTypeMapper _transactionTypeMapper;

        public TransactionMapper(ITransactionTypeMapper transactionTypeMapper)
        {
            _transactionTypeMapper = transactionTypeMapper;
        }

        public IFidelityTransaction CreateTransaction(string row, IDictionary<FidelityCsvColumn, int> headers)
        {
            var values = row.Split(',');
            var actionText = ParseStringField(values[headers[FidelityCsvColumn.Action]]);
            return new FidelityTransaction(
                ParseDateField(values[headers[FidelityCsvColumn.RunDate]]),
                ParseStringField(values[headers[FidelityCsvColumn.Account]]),
                actionText,
                _transactionTypeMapper.Map(actionText),
                ParseStringField(values[headers[FidelityCsvColumn.Symbol]]),
                ParseStringField(values[headers[FidelityCsvColumn.SecurityDescription]]),
                ParseStringField(values[headers[FidelityCsvColumn.SecurityType]]),
                ParseDecimalField(values[headers[FidelityCsvColumn.Quantity]]),
                ParseDecimalField(values[headers[FidelityCsvColumn.Price]]),
                ParseDecimalField(values[headers[FidelityCsvColumn.Commission]]),
                ParseDecimalField(values[headers[FidelityCsvColumn.Fees]]),
                ParseDecimalField(values[headers[FidelityCsvColumn.AccruedInterest]]),
                ParseDecimalField(values[headers[FidelityCsvColumn.Amount]]),
                ParseDateField(values[headers[FidelityCsvColumn.SettlementDate]]));
        }

        private decimal? ParseDecimalField(string decimalString)
        {
            return string.IsNullOrWhiteSpace(decimalString)
                ? new decimal?()
                : decimal.Parse(decimalString.Trim());
        }

        private string ParseStringField(string rawString)
        {
            return rawString.Trim();
        }

        private DateTime? ParseDateField(string dateString)
        {
            return string.IsNullOrWhiteSpace(dateString)
                ? new DateTime?()
                : DateTime.Parse(dateString);
        }
    }
}