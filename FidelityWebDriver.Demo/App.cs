using System;
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
                    password=> { _fidelityConfiguration.Password = password; }
                },
                {
                    "s|save", "indicates options should be persisted.",
                    save=> { _shouldPersistOptions = true; }
                }
            };
            _fidelityConfiguration = fidelityConfiguration;
            _transactionManager = transactionManager;
        }

        public void Run(string[] args)
        {
            _optionSet.Parse(args);
            if (_shouldPersistOptions)
            {
                _fidelityConfiguration.Write();
            }
            _optionSet.WriteOptionDescriptions(Console.Out);

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