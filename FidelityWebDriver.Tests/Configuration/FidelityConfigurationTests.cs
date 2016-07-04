using System;
using System.IO;
using System.IO.IsolatedStorage;
using NUnit.Framework;
using Sonneville.Configuration;
using Sonneville.FidelityWebDriver.Configuration;

namespace Sonneville.FidelityWebDriver.Tests.Configuration
{
    [TestFixture]
    public class FidelityConfigurationTests
    {
        private IsolatedStorageFile _isolatedStore;
        private FidelityConfiguration _config;
        private ConfigStore _configStore;

        [SetUp]
        public void Setup()
        {
            _isolatedStore = IsolatedStorageFile.GetUserStoreForAssembly();

            _configStore = new ConfigStore(_isolatedStore);
            _config = _configStore.Get<FidelityConfiguration>();
        }

        [TearDown]
        public void Teardown()
        {
            _configStore.Clear();
        }

        [Test]
        public void ShouldCreateProviderWithPasswordEncrypted()
        {
            var provider = _config.Provider;

            var actualPropertiesToEncrypt = provider.PropertiesToEncrypt.Split(',');
            Assert.Contains("Password", actualPropertiesToEncrypt);
        }

        [Test]
        public void ShouldCreateSameEncryptionKey()
        {
            var provider1 = _configStore.Get<FidelityConfiguration>().Provider;
            var provider2 = _configStore.Get<FidelityConfiguration>().Provider;

            Assert.AreNotSame(provider1, provider2);
            Assert.AreEqual(provider1.EncryptionKey, provider2.EncryptionKey);
        }

        [Test]
        public void ShouldHaveDefaultDownloadPath()
        {
            var actualDownloadPath = _config.DownloadPath;

            var expectedDownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Downloads", "Accounts_History.csv");
            Assert.AreEqual(expectedDownloadPath, actualDownloadPath);
        }
    }
}