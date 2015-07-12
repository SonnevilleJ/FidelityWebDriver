using System;
using System.Collections.Generic;
using NDesk.Options;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Managers;

namespace Sonneville.FidelityWebDriver.Demo
{
    public class App : IApp
    {
        private readonly ITransactionManager _transactionManager;
        private readonly FidelityConfiguration _fidelityConfiguration;
        private readonly OptionSet _optionSet;
        private bool _shouldPersistOptions;
        private bool _shouldShowHelp;

        public App(ITransactionManager transactionManager, FidelityConfiguration fidelityConfiguration)
        {
            _optionSet = new OptionSet
            {
                {
                    "u|username=", "the username to use when logging into Fidelity.",
                    username => { _fidelityConfiguration.Username = username; }
                },
                {
                    "p|password=", "the password to use when logging into Fidelity",
                    password => { _fidelityConfiguration.Password = password; }
                },
                {
                    "s|save", "indicates options should be persisted.",
                    save => { _shouldPersistOptions = true; }
                },
                {
                    "h|help", "shows this message and exits.",
                    help => { _shouldShowHelp = true; }
                },
            };
            _fidelityConfiguration = fidelityConfiguration;
            _transactionManager = transactionManager;
        }

        public void Run(IEnumerable<string> args)
        {
            _optionSet.Parse(args);
            if (_shouldShowHelp)
            {
                _optionSet.WriteOptionDescriptions(Console.Out);
                return;
            }
            if (_shouldPersistOptions)
            {
                _fidelityConfiguration.Write();
            }

            _transactionManager.DownloadTransactions();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var transactionManager = _transactionManager;
                if (transactionManager != null) transactionManager.Dispose();
            }
        }
    }
}