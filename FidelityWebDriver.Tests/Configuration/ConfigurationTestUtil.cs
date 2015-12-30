using Sonneville.FidelityWebDriver.Configuration;

namespace Sonneville.FidelityWebDriver.Tests.Configuration
{
    public class ConfigurationTestUtil
    {
        public static void ClearPersistedConfiguration()
        {
            var fidelityConfiguration = new FidelityConfiguration();
            fidelityConfiguration.Initialize();

            fidelityConfiguration.Username = null;
            fidelityConfiguration.Password = null;
            fidelityConfiguration.DownloadPath = null;
            fidelityConfiguration.Write();
        }
    }
}