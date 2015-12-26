using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Positions
{
    public interface IPositionsManager : IManager
    {
        IEnumerable<IAccountSummary> GetAccounts();
    }
}