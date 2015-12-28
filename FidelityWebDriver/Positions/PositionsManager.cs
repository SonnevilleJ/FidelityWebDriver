using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Login;
using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Positions
{
    public class PositionsManager : IPositionsManager
    {
        private ISiteNavigator _siteNavigator;
        private ILoginManager _loginManager;

        public PositionsManager(ISiteNavigator siteNavigator, ILoginManager loginManager)
        {
            _siteNavigator = siteNavigator;
            _loginManager = loginManager;
        }

        public IEnumerable<IAccountSummary> GetAccountSummaries()
        {
            return _loginManager.EnsureLoggedIn().GoToPositionsPage().GetAccountSummaries();
        }

        public IEnumerable<IAccountDetails> GetAccountDetails()
        {
            return _loginManager.EnsureLoggedIn().GoToPositionsPage().GetAccountDetails();
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