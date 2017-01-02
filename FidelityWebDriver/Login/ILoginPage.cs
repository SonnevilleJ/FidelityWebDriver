using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Login
{
    public interface ILoginPage : IPage
    {
        void LogIn(string username, string password);
    }
}