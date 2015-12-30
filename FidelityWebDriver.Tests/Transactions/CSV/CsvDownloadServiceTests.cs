using System.IO;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Tests.Configuration;
using Sonneville.FidelityWebDriver.Transactions.CSV;

namespace Sonneville.FidelityWebDriver.Tests.Transactions.Csv
{
    [TestFixture]
    public class CsvDownloadServiceTests
    {
        private CsvDownloadService _downloadService;
        private string _tempFile;
        private string _fileContents;
        private FidelityConfiguration _fidelityConfiguration;

        [SetUp]
        public void Setup()
        {
            _fileContents = @"line 1
line 2
line 3";
            _tempFile = SetupTempFile(_fileContents);

            ConfigurationTestUtil.ClearPersistedConfiguration();

            _fidelityConfiguration = new FidelityConfiguration();
            _fidelityConfiguration.Initialize();
            _fidelityConfiguration.DownloadPath = _tempFile;

            _downloadService = new CsvDownloadService(_fidelityConfiguration);
        }

        [TearDown]
        public void TearDown()
        {
            ConfigurationTestUtil.ClearPersistedConfiguration();

            ClearTempFile(_tempFile);
        }

        [Test]
        public void ShouldCleanupExistingFile()
        {
            _downloadService.Cleanup();

            Assert.False(File.Exists(_tempFile));
        }

        [Test]
        public void ShouldReturnFileContents()
        {
            var content = _downloadService.GetDownloadedContent();

            Assert.AreEqual(_fileContents, content);
        }

        [Test]
        public void ShouldReturnFileContentsAfterConfigurationIsChanged()
        {
            ClearTempFile(_tempFile);
            _fidelityConfiguration.DownloadPath = SetupTempFile(_fileContents);

            var content = _downloadService.GetDownloadedContent();

            Assert.AreEqual(_fileContents, content);
        }

        private string SetupTempFile(string fileContents)
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, fileContents);
            return tempFile;
        }

        private void ClearTempFile(string tempFile)
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}