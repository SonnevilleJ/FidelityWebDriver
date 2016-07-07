using System.IO;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.Utilities;

namespace Sonneville.FidelityWebDriver.Transactions.CSV
{
    public class CsvDownloadService : ICsvDownloadService
    {
        private readonly FidelityConfiguration _fidelityConfiguration;
        private readonly ISleepUtil _sleepUtil;

        public CsvDownloadService(FidelityConfiguration fidelityConfiguration, ISleepUtil sleepUtil)
        {
            _fidelityConfiguration = fidelityConfiguration;
            _sleepUtil = sleepUtil;
        }

        public string GetDownloadedContent()
        {
            _sleepUtil.Sleep(2000);
            return File.ReadAllText(_fidelityConfiguration.DownloadPath);
        }

        public void Cleanup()
        {
            _sleepUtil.Sleep(1000);
            if (File.Exists(_fidelityConfiguration.DownloadPath))
            {
                File.Delete(_fidelityConfiguration.DownloadPath);
            }
        }
    }
}