namespace Sonneville.FidelityWebDriver.Pages
{
    public interface ILoginPage : IPage
    {
        void LogIn(string username, string password);
    }
}