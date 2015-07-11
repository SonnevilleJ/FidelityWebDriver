using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver.Managers
{
    public class LoginManager : ILoginManager
    {
        private readonly ISiteNavigator _siteNavigator;
        private readonly FidelityConfiguration _fidelityConfiguration;

        public LoginManager(ISiteNavigator siteNavigator, FidelityConfiguration fidelityConfiguration)
        {
            _siteNavigator = siteNavigator;
            _fidelityConfiguration = fidelityConfiguration;
        }

        public bool IsLoggedIn { get; private set; }

        public ISummaryPage LogIn()
        {
            var loginPage = _siteNavigator.GoToLoginPage();

            var summaryPage = loginPage.LogIn(_fidelityConfiguration.Username, _fidelityConfiguration.Password);
            IsLoggedIn = true;
            return summaryPage;
        }

        public void EnsureLoggedIn()
        {
            if (!IsLoggedIn)
            {
                LogIn();
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
                var siteNavigator = _siteNavigator;
                if(siteNavigator != null) siteNavigator.Dispose();
            }
        }
    }
}