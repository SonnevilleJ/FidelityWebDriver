namespace Sonneville.FidelityWebDriver.Pages
{
    public interface IPageFactory
    {
        T GetPage<T>() where T : IPage;
    }
}