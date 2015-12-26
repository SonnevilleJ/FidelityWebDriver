using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Positions
{
    public interface IPositionsPage : IPage
    {
        IEnumerable<IAccount> GetAccountSummaries();
    }
}