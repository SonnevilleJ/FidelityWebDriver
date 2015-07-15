using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Pages
{
    public interface IPositionsPage : IPage
    {
        IEnumerable<IAccount> BuildAccounts();
    }
}