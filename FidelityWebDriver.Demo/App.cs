using Sonneville.FidelityWebDriver.Managers;

namespace Sonneville.FidelityWebDriver.Demo
{
    public class App : IApp
    {
        private readonly ITransactionManager _transactionManager;

        public App(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        public void Run(string[] args)
        {
            _transactionManager.DownloadTransactions();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transactionManager.Dispose();
            }
        }
    }
}