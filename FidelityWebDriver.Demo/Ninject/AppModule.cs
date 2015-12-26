using System;
using System.IO;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Transactions.CSV;

namespace Sonneville.FidelityWebDriver.Demo.Ninject
{
    public class AppModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(syntax => syntax.FromAssembliesMatching("Sonneville.*")
                .SelectAllClasses()
                .BindDefaultInterface()
                .Configure(configurationAction => configurationAction.InSingletonScope()));

            var fidelityConfiguration = new FidelityConfiguration();
            fidelityConfiguration.Initialize();
            Kernel.Bind<FidelityConfiguration>().ToConstant(fidelityConfiguration);

            var downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Downloads", "Accounts_History.csv");

            Kernel.Rebind<ICsvDownloadService>()
                .To<CsvDownloadService>()
                .WithConstructorArgument(downloadPath);
        }
    }
}