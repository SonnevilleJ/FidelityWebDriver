using System;
using System.IO;
using System.IO.IsolatedStorage;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Configuration;
using Westwind.Utilities.Configuration;

namespace Sonneville.FidelityWebDriver.Tests.Configuration
{
    [TestFixture]
    public class FidelityConfigurationTests
    {
        private IsolatedStorageFile _isolatedStore;

        private class FidelityConfigurationInheritor : FidelityConfiguration
        {
            public IConfigurationProvider CreateDefaultProvider(string sectionName, object configData)
            {
                return OnCreateDefaultProvider(sectionName, configData);
            }
        }

        [SetUp]
        public void Setup()
        {
            _isolatedStore = IsolatedStorageFile.GetUserStoreForAssembly();
        }

        [TearDown]
        public void Teardown()
        {
            ConfigurationTestUtil.ClearPersistedConfiguration(_isolatedStore);
        }

        [Test]
        public void ShouldCreateProviderWithPasswordEncrypted()
        {
            var provider = new FidelityConfigurationInheritor().CreateDefaultProvider(null, null);

            var actualPropertiesToEncrypt = provider.PropertiesToEncrypt.Split(',');
            Assert.Contains("Password", actualPropertiesToEncrypt);
        }

        [Test]
        public void ShouldCreateSameEncryptionKey()
        {
            var provider1 = new FidelityConfigurationInheritor().CreateDefaultProvider(null, null);
            var provider2 = new FidelityConfigurationInheritor().CreateDefaultProvider(null, null);

            Assert.AreNotSame(provider1, provider2);
            Assert.AreEqual(provider1.EncryptionKey, provider2.EncryptionKey);
        }

        [Test]
        public void ShouldHaveDefaultDownloadPath()
        {
            var configuration = FidelityConfiguration.Initialize(_isolatedStore);

            var actualDownloadPath = configuration.DownloadPath;

            var expectedDownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Downloads", "Accounts_History.csv");
            Assert.AreEqual(expectedDownloadPath, actualDownloadPath);
        }
    }
}