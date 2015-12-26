﻿using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Login
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

        private ISummaryPage LogIn()
        {
            var loginPage = _siteNavigator.GoTo<ILoginPage>();

            var summaryPage = loginPage.LogIn(_fidelityConfiguration.Username, _fidelityConfiguration.Password);
            IsLoggedIn = true;
            return summaryPage;
        }

        public ISummaryPage EnsureLoggedIn()
        {
            if (!IsLoggedIn)
            {
                return LogIn();
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
                var siteNavigator = _siteNavigator;
                if(siteNavigator != null) siteNavigator.Dispose();
            }
        }
    }
}