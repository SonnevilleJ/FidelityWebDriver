using Sonneville.FidelityWebDriver.Login;

namespace Sonneville.FidelityWebDriver.Navigation
{
    public interface IHomePage : IPage
    {
        ILoginPage GoToLoginPage();
    }
}