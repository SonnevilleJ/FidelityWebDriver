namespace Sonneville.FidelityWebDriver.CSV
{
    public interface ICsvDownloadService
    {
        string GetDownloadedContent();

        void Cleanup();
    }
}