namespace Sonneville.FidelityWebDriver.Transactions.CSV
{
    public interface ICsvDownloadService
    {
        string GetDownloadedContent();

        void Cleanup();
    }
}