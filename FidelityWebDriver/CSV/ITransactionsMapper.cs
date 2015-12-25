using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.CSV
{
    public interface ITransactionsMapper
    {
        IList<IFidelityTransaction> ParseCsv(string downloadPath);
    }
}