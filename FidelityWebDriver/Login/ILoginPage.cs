using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Login
{
    public interface ILoginPage : IPage
    {
        ISummaryPage LogIn(string username, string password);
    }
}