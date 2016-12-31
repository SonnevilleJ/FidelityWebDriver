using System.Collections.Generic;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Transactions
{
    public interface IHistoryTransactionParser
    {
        IEnumerable<IFidelityTransaction> ParseFidelityTransactions(IWebElement historyRoot);
    }
}