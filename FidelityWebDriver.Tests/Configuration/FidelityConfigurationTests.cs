using System;
using System.IO;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Configuration;
using Westwind.Utilities.Configuration;

namespace Sonneville.FidelityWebDriver.Tests.Configuration
{
    [TestFixture]
    public class FidelityConfigurationTests
    {
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
            ConfigurationTestUtil.ClearPersistedConfiguration();
        }

        [TearDown]
        public void Teardown()
        {
            ConfigurationTestUtil.ClearPersistedConfiguration();
        }

        [Test]
        public void ShouldCreateProviderWithPasswordEncrypted()
        {
            var inheritor = new FidelityConfigurationInheritor();
            var provider = inheritor.CreateDefaultProvider(null, null);

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
            var configuration = new FidelityConfiguration();
            configuration.Initialize();

            var actualDownloadPath = configuration.DownloadPath;

            var expectedDownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Downloads", "Accounts_History.csv");
            Assert.AreEqual(expectedDownloadPath, actualDownloadPath);
        }
    }
}