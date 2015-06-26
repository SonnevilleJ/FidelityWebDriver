namespace Sonneville.FidelityWebDriver.Managers
{
    public class TransactionManager : ITransactionManager
    {
        private readonly ISiteNavigator _siteNavigator;

        public TransactionManager(ISiteNavigator siteNavigator)
        {
            _siteNavigator = siteNavigator;
        }

        public void DownloadTransactions()
        {
            _siteNavigator.GoToHomepage();

            if (Settings.Default.AutoCloseSelenium)
            {
                _siteNavigator.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var fidelityDriver = _siteNavigator;
                if (fidelityDriver != null) fidelityDriver.Dispose();
            }
        }
    }
}