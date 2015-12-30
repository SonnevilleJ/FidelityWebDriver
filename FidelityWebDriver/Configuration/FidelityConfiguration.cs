using System;
using System.IO;
using System.Reflection;
using Sonneville.Utilities;
using Westwind.Utilities.Configuration;

namespace Sonneville.FidelityWebDriver.Configuration
{
    public class FidelityConfiguration : AppConfiguration
    {
        protected override IConfigurationProvider OnCreateDefaultProvider(string sectionName, object configData)
        {
            var encryptionKey =
                $"{Environment.MachineName}-{Environment.UserName}-{Assembly.GetExecutingAssembly().ManifestModule}";
            return new IsolatedStorageConfigurationProvider<FidelityConfiguration>
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