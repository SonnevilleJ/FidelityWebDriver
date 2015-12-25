using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.CSV
{
    public interface ITransactionMapper
    {
        IFidelityTransaction CreateTransaction(string row, IDictionary<FidelityCsvColumn, int> headers);
    }
}