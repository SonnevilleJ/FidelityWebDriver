using System.IO.IsolatedStorage;

namespace Sonneville.FidelityWebDriver.Tests.Configuration
{
    public class ConfigurationTestUtil
    {
        public static void ClearPersistedConfiguration(IsolatedStorageFile isolatedStore)
        {
            foreach (var fileName in isolatedStore.GetFileNames())
            {
                isolatedStore.DeleteFile(fileName);
            }
        }
    }
}