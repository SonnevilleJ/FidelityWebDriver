namespace Sonneville.FidelityWebDriver.Managers
{
    public class TransactionManager : ITransactionManager
    {
        private readonly ISiteNavigator _siteNavigator;
        private readonly ILoginManager _loginManager;

        public TransactionManager(ISiteNavigator siteNavigator, ILoginManager loginManager)
        {
            _siteNavigator = siteNavigator;
            _loginManager = loginManager;
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