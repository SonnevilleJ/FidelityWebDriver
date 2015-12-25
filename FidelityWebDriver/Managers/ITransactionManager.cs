namespace Sonneville.FidelityWebDriver.Managers
{
    public interface ITransactionManager : IManager
    {
        void DownloadTransactionHistory();
    }
}