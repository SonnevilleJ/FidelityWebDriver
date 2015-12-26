namespace Sonneville.FidelityWebDriver.Navigation
{
    public interface IPageFactory
    {
        T GetPage<T>() where T : IPage;
    }
}