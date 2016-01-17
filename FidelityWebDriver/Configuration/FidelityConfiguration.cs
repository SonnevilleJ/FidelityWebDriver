using System;
using System.IO;
using System.Reflection;
using Westwind.Utilities.Configuration;

namespace Sonneville.FidelityWebDriver.Configuration
{
    public class FidelityConfiguration : AppConfiguration
    {
        protected override void OnInitialize(IConfigurationProvider provider, string sectionName, object configData)
        {
            provider.PropertiesToEncrypt = nameof(Password);
            provider.EncryptionKey =
                $"{Environment.MachineName}-{Environment.UserName}-{Assembly.GetExecutingAssembly().ManifestModule}";

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