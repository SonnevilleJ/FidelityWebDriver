using System;
using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Login;
using Sonneville.FidelityWebDriver.Navigation;
using Sonneville.FidelityWebDriver.Transactions.CSV;

namespace Sonneville.FidelityWebDriver.Transactions
{
    public class TransactionManager : ITransactionManager
    {
        private ISiteNavigator _siteNavigator;
        private ILoginManager _loginManager;
        private readonly ITransactionsMapper _transactionsMapper;

        public TransactionManager(ISiteNavigator siteNavigator, ILoginManager loginManager,
            ITransactionsMapper transactionsMapper)
        {
            _siteNavigator = siteNavigator;
            _loginManager = loginManager;
            _transactionsMapper = transactionsMapper;
        }

        public IList<IFidelityTransaction> DownloadTransactionHistory()
        {
            _loginManager.EnsureLoggedIn();
            var activityPage = _siteNavigator.GoTo<IActivityPage>();
            var downloadPath = activityPage.DownloadHistory(DateTime.MinValue, DateTime.Today);

            return _transactionsMapper.ParseCsv(downloadPath);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _siteNavigator?.Dispose();
                _siteNavigator = null;

                _loginManager?.Dispose();
                _loginManager = null;
            }
        }
    }
}