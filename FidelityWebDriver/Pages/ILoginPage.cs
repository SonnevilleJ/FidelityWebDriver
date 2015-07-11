namespace Sonneville.FidelityWebDriver.Pages
{
    public interface ILoginPage : IPage
    {
        ISummaryPage LogIn(string username, string password);
    }
}