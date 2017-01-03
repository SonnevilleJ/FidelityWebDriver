namespace Sonneville.FidelityWebDriver.Login
{
    public interface ILoginManager : IManager
    {
        bool IsLoggedIn { get; }
        void EnsureLoggedIn();
    }
}