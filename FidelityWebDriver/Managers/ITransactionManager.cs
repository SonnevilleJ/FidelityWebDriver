using System;

namespace Sonneville.FidelityWebDriver.Managers
{
    public interface ITransactionManager : IDisposable
    {
        void DownloadTransactions();
    }
}