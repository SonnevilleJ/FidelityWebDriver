using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using Sonneville.Utilities;
using Westwind.Utilities.Configuration;

namespace Sonneville.FidelityWebDriver.Configuration
{
    public class FidelityConfiguration : AppConfiguration
    {
        public static FidelityConfiguration Initialize(IsolatedStorageFile isolatedStore)
        {
            var configuration = new FidelityConfiguration {Store = isolatedStore};
            configuration.Initialize();
            return configuration;
        }

        private IsolatedStorageFile Store { get; set; }

        protected override IConfigurationProvider OnCreateDefaultProvider(string sectionName, object configData)
        {
            var encryptionKey =
                $"{Environment.MachineName}-{Environment.UserName}-{Assembly.GetExecutingAssembly().ManifestModule}";
            return new IsolatedStorageConfigurationProvider<FidelityConfiguration>(Store)
            {
                PropertiesToEncrypt = nameof(Password),
                EncryptionKey = encryptionKey
            };
        }

        protected override void OnInitialize(IConfigurationProvider provider, string sectionName, object configData)
        {
            base.OnInitialize(provider, sectionName, configData);

            if (string.IsNullOrWhiteSpace(DownloadPath))
            {
                DownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Downloads", "Accounts_History.csv");
            }
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string DownloadPath { get; set; }
    }
}