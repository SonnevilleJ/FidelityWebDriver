using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Managers
{
    public interface IPositionsManager : IManager
    {
        IEnumerable<IAccount> GetAccounts();
    }
}