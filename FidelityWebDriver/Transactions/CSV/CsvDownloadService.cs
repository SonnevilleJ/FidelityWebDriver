using System.IO;
using System.Threading;
using Sonneville.FidelityWebDriver.Configuration;

namespace Sonneville.FidelityWebDriver.Transactions.CSV
{
    public class CsvDownloadService : ICsvDownloadService
    {
        private readonly FidelityConfiguration _fidelityConfiguration;

        public CsvDownloadService(FidelityConfiguration fidelityConfiguration)
        {
            _fidelityConfiguration = fidelityConfiguration;
        }

        public string GetDownloadedContent()
        {
            Thread.Sleep(2000);
            return File.ReadAllText(_fidelityConfiguration.DownloadPath);
        }

        public void Cleanup()
        {
            Thread.Sleep(1000);
            if (File.Exists(_fidelityConfiguration.DownloadPath))
            {
                File.Delete(_fidelityConfiguration.DownloadPath);
            }
        }
    }
}