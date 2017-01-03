namespace Sonneville.FidelityWebDriver.Navigation.Pages
{
    public interface ILoginPage : IPage
    {
        void LogIn(string username, string password);
    }
}