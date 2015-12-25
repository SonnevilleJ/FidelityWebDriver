using System.IO;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.CSV;

namespace Sonneville.FidelityWebDriver.Tests.CSV
{
    [TestFixture]
    public class CsvDownloadServiceTests
    {
        private CsvDownloadService _downloadService;
        private string _tempFile;
        private string _fileContents;

        [SetUp]
        public void Setup()
        {
            _tempFile = Path.GetTempFileName();

            _fileContents = @"line 1
line 2
line 3";
            File.WriteAllText(_tempFile, _fileContents);

            _downloadService = new CsvDownloadService(_tempFile);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_tempFile))
            {
                File.Delete(_tempFile);
            }
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
    }
}