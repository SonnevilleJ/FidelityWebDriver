using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Login
{
    public interface ILoginManager : IManager
    {
        bool IsLoggedIn { get; }
        ISummaryPage EnsureLoggedIn();
    }
}