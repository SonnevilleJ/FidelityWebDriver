using System.IO;
using System.Threading;

namespace Sonneville.FidelityWebDriver.CSV
{
    public class CsvDownloadService : ICsvDownloadService
    {
        private readonly string _downloadFilePath;

        public CsvDownloadService(string downloadFilePath)
        {
            _downloadFilePath = downloadFilePath;
        }

        public string GetDownloadedContent()
        {
            Thread.Sleep(2000);
            return File.ReadAllText(_downloadFilePath);
        }

        public void Cleanup()
        {
            Thread.Sleep(1000);
            if (File.Exists(_downloadFilePath))
            {
                File.Delete(_downloadFilePath);
            }
        }
    }
}