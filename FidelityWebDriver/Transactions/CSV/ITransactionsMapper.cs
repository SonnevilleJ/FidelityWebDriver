using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Transactions.CSV
{
    public interface ITransactionsMapper
    {
        IList<IFidelityTransaction> ParseCsv(string csvContent);
    }
}