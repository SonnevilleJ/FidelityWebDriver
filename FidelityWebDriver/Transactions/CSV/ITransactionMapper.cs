using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Transactions.CSV
{
    public interface ITransactionMapper
    {
        IFidelityTransaction CreateTransaction(string row, IDictionary<FidelityCsvColumn, int> headers);
    }
}