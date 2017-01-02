using log4net;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Login
{
    public class LoginManager : ILoginManager
    {
        private readonly ILog _log;
        private ISiteNavigator _siteNavigator;
        private readonly FidelityConfiguration _fidelityConfiguration;

        public LoginManager(ILog log, ISiteNavigator siteNavigator, FidelityConfiguration fidelityConfiguration)
        {
            _log = log;
            _siteNavigator = siteNavigator;
            _fidelityConfiguration = fidelityConfiguration;
        }

        public bool IsLoggedIn { get; private set; }

        private void LogIn()
        {
            _log.Info("Logging in to Fidelity...");

            _siteNavigator.GoTo<ILoginPage>().LogIn(_fidelityConfiguration.Username, _fidelityConfiguration.Password);
            IsLoggedIn = true;
        }

        public ISummaryPage EnsureLoggedIn()
        {
            if (!IsLoggedIn)
            {
                LogIn();
            }
            return _siteNavigator.GoTo<ISummaryPage>();
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
            }
        }
    }
}