using System;
using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Navigation.Pages
{
    public interface IActivityPage : IPage
    {
        IEnumerable<IFidelityTransaction> GetTransactions(DateTime startDate, DateTime endDate);
    }
}