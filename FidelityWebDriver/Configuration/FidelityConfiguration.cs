using System;
using System.IO;

namespace Sonneville.FidelityWebDriver.Configuration
{
    public class FidelityConfiguration
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string DownloadPath { get; set; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Downloads", "Accounts_History.csv"
        );
    }
}
