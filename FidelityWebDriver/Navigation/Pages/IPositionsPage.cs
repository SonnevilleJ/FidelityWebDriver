using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Navigation.Pages
{
    public interface IPositionsPage : IPage
    {
        IEnumerable<IAccountSummary> GetAccountSummaries();

        IEnumerable<IAccountDetails> GetAccountDetails();
    }
}