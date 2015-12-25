using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.CSV
{
    public class TransactionsMapper : ITransactionsMapper
    {
        private readonly IFidelityCsvColumnMapper _fidelityCsvColumnMapper;
        private readonly ITransactionMapper _transactionMapper;

        public TransactionsMapper(IFidelityCsvColumnMapper fidelityCsvColumnMapper, ITransactionMapper transactionMapper)
        {
            _fidelityCsvColumnMapper = fidelityCsvColumnMapper;
            _transactionMapper = transactionMapper;
        }

        public IList<IFidelityTransaction> ParseCsv(string csvContent)
        {
            var rows = csvContent.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            var headers = _fidelityCsvColumnMapper.GetColumnMappings(rows.Take(1).Single());
            return rows.Skip(1).Select(row => _transactionMapper.CreateTransaction(row, headers)).ToList();
        }
    }
}