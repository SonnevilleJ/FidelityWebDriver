using System;
using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Transactions
{
    public interface IActivityPage : IPage
    {
        IEnumerable<IFidelityTransaction> GetTransactions(DateTime startDate, DateTime endDate);
    }
}