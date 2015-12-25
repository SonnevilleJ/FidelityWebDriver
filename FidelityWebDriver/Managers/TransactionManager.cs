using System;
using System.Collections.Generic;
using Sonneville.FidelityWebDriver.CSV;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver.Managers
{
    public class TransactionManager : ITransactionManager
    {
        private readonly ISiteNavigator _siteNavigator;
        private readonly ILoginManager _loginManager;
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
                var fidelityDriver = _siteNavigator;
                if (fidelityDriver != null) fidelityDriver.Dispose();
            }
        }
    }
}