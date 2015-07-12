using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver.Managers
{
    public interface ILoginManager : IManager
    {
        bool IsLoggedIn { get; }
        ISummaryPage EnsureLoggedIn();
    }
}