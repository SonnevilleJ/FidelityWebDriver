using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.CSV
{
    public interface IFidelityCsvParser
    {
        IList<FidelityTransaction> ParseCsv(string downloadPath);
    }
}