namespace Sonneville.FidelityWebDriver.Managers
{
    public interface ILoginManager : IManager
    {
        bool IsLoggedIn { get; }
        void LogIn();
        void EnsureLoggedIn();
    }
}