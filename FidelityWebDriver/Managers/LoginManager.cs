namespace Sonneville.FidelityWebDriver.Managers
{
    public class LoginManager : ILoginManager
    {
        private readonly ISiteNavigator _siteNavigator;

        public LoginManager(ISiteNavigator siteNavigator)
        {
            _siteNavigator = siteNavigator;
        }

        public bool IsLoggedIn { get; private set; }

        public void LogIn()
        {
            var loginPage = _siteNavigator.GoToLoginPage();
            loginPage.LogIn(Settings.Default.Username, Settings.Default.Password);
            IsLoggedIn = true;
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