using System.IO;
using System.IO.IsolatedStorage;
using Moq;
using NUnit.Framework;
using Sonneville.Configuration;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Transactions.CSV;
using Sonneville.Utilities;

namespace Sonneville.FidelityWebDriver.Tests.Transactions.Csv
{
    [TestFixture]
    public class CsvDownloadServiceTests
    {
        private CsvDownloadService _downloadService;
        private string _tempFile;
        private string _fileContents;
        private FidelityConfiguration _fidelityConfiguration;
        private Mock<ISleepUtil> _sleepUtilMock;

        [SetUp]
        public void Setup()
        {
            _fileContents = @"line 1
line 2
line 3";
            _tempFile = SetupTempFile(_fileContents);

            _sleepUtilMock = new Mock<ISleepUtil>();

            _fidelityConfiguration = new ConfigStore(IsolatedStorageFile.GetUserStoreForAssembly()).Get<FidelityConfiguration>();
            _fidelityConfiguration.DownloadPath = _tempFile;

            _downloadService = new CsvDownloadService(_fidelityConfiguration, _sleepUtilMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            ClearTempFile(_tempFile);
        }

        [Test]
        public void ShouldCleanupExistingFile()
        {
            _downloadService.Cleanup();

            Assert.False(File.Exists(_tempFile));
            _sleepUtilMock.Verify(util => util.Sleep(1000));
        }

        [Test]
        public void ShouldReturnFileContents()
        {
            var content = _downloadService.GetDownloadedContent();

            Assert.AreEqual(_fileContents, content);
            _sleepUtilMock.Verify(util => util.Sleep(2000));
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