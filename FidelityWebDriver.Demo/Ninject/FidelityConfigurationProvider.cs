using System;
using System.IO;
using Nini.Config;
using Ninject.Activation;
using Sonneville.FidelityWebDriver.Configuration;

namespace Sonneville.FidelityWebDriver.Demo.Ninject
{
    public class FidelityConfigurationProvider : IProvider<FidelityConfiguration>
    {
        private static readonly FidelityConfiguration Instance = new FidelityConfiguration();

        public object Create(IContext context)
        {
            if (File.Exists("./demo.ini"))
            {
                var config = new IniConfigSource("./demo.ini").Configs["Fidelity"];
                Instance.Username = config.Get("Username");
                Instance.Password = config.Get("Password");
            }
            return Instance;
        }

        public Type Type => typeof(FidelityConfiguration);
    }
}
