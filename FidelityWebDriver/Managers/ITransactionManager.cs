using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Managers
{
    public interface ITransactionManager : IManager
    {
        IList<FidelityTransaction> DownloadTransactionHistory();
    }
}