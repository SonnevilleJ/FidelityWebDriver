using System;
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

        public string Username { get; set; }

        public string Password { get; set; }
    }
}