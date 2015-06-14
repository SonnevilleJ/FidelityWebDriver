﻿using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace Sonneville.TradingHistoryCsvDownloader.Ninject
{
    public class AppModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(
                syntax =>
                    syntax.FromAssembliesMatching("Sonneville.*")
                        .SelectAllClasses()
                        .BindDefaultInterface()
                        .Configure(configurationAction => configurationAction.InSingletonScope()));
        }
    }
}