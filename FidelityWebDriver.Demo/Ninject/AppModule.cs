using System.IO.IsolatedStorage;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.Configuration;

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

            var configStore = new ConfigStore(IsolatedStorageFile.GetUserStoreForAssembly());
            Kernel.Bind<FidelityConfiguration>().ToConstant(configStore.Get<FidelityConfiguration>());
        }
    }
}