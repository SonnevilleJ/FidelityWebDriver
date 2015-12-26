using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Transactions
{
    public interface ITransactionManager : IManager
    {
        IList<IFidelityTransaction> DownloadTransactionHistory();
    }
}