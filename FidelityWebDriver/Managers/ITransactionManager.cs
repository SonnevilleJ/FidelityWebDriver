using System;

namespace Sonneville.FidelityWebDriver.Managers
{
    public interface ITransactionManager : IManager
    {
        void DownloadTransactions();
    }
}