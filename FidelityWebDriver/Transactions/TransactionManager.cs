using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Login;
using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Transactions
{
    public class TransactionManager : ITransactionManager
    {
        private ISiteNavigator _siteNavigator;
        private ILoginManager _loginManager;

        public TransactionManager(ISiteNavigator siteNavigator, ILoginManager loginManager)
        {
            _siteNavigator = siteNavigator;
            _loginManager = loginManager;
        }

        public IList<IFidelityTransaction> DownloadTransactionHistory(DateTime startDate, DateTime endDate)
        {
            _loginManager.EnsureLoggedIn();
            var activityPage = _siteNavigator.GoTo<IActivityPage>();
            return activityPage.GetTransactions(startDate, endDate).ToList();
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