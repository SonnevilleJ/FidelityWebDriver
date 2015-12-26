using System.Collections.Generic;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Login;
using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Positions
{
    public class PositionsManager : IPositionsManager
    {
        private readonly ISiteNavigator _siteNavigator;
        private readonly ILoginManager _loginManager;

        public PositionsManager(ISiteNavigator siteNavigator, ILoginManager loginManager)
        {
            _siteNavigator = siteNavigator;
            _loginManager = loginManager;
        }

        public IEnumerable<IAccount> GetAccounts()
        {
            return _loginManager.EnsureLoggedIn().GoToPositionsPage().BuildAccounts();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var siteNavigator = _siteNavigator;
                if (siteNavigator != null) siteNavigator.Dispose();

                var loginManager = _loginManager;
                if (loginManager != null) loginManager.Dispose();
            }
        }
    }
}