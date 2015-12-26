using System.Collections.Generic;

namespace Sonneville.FidelityWebDriver.Transactions.CSV
{
    public interface IFidelityCsvColumnMapper
    {
        FidelityCsvColumn GetHeader(string text);

        IDictionary<FidelityCsvColumn, int> GetColumnMappings(string headerRow);
    }
}