using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NDesk.Options;
using Nini.Config;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Positions;
using Sonneville.FidelityWebDriver.Transactions;

namespace Sonneville.FidelityWebDriver.Demo
{
    public class App : IApp
    {
        private IPositionsManager _positionsManager;

        private ITransactionManager _transactionManager;

        private readonly TransactionTranslator _transactionTranslator;

        private readonly FidelityConfiguration _fidelityConfiguration;
        private readonly OptionSet _optionSet;
        private bool _shouldPersistOptions;
        private bool _shouldShowHelp;

        public App(IPositionsManager positionsManager,
            ITransactionManager transactionManager,
            FidelityConfiguration fidelityConfiguration,
            TransactionTranslator transactionTranslator)
        {
            _fidelityConfiguration = fidelityConfiguration;
            _positionsManager = positionsManager;
            _transactionManager = transactionManager;
            _transactionTranslator = transactionTranslator;
            _optionSet = new OptionSet
            {
                {
                    "u|username=", "the username to use when logging into Fidelity.",
                    username => { _fidelityConfiguration.Username = username; }
                },
                {
                    "p|password=", "the password to use when logging into Fidelity.",
                    password => { _fidelityConfiguration.Password = password; }
                },
                {
                    "s|save", "indicates options should be persisted to demo.ini file.",
                    save => { _shouldPersistOptions = true; }
                },
                {
                    "h|help", "shows this message and exits.",
                    help => { _shouldShowHelp = true; }
                },
            };
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
                File.Create("./demo.ini").Dispose();
                var iniConfigSource = new IniConfigSource("./demo.ini");
                iniConfigSource.AddConfig("Fidelity");
                var config = iniConfigSource.Configs["Fidelity"];
                config.Set("DownloadPath", _fidelityConfiguration.DownloadPath);
                config.Set("Username", _fidelityConfiguration.Username);
                config.Set("Password", _fidelityConfiguration.Password);
                iniConfigSource.Save();
            }
            if (string.IsNullOrEmpty(_fidelityConfiguration.Username))
            {
                Console.Write("Please enter a username for Fidelity.com: ");
                _fidelityConfiguration.Username = Console.ReadLine();
                Console.Write("Please enter a password for Fidelity.com: ");
                _fidelityConfiguration.Password = Console.ReadLine();
            }

            Console.WriteLine("Reading account summaries.....");
            PrintAccountSummaries(_positionsManager.GetAccountSummaries().ToList());
            PrintSeparator();
            Console.WriteLine("Reading account details.......");
            PrintAccountDetails(_positionsManager.GetAccountDetails().ToList());
            PrintSeparator();
            Console.WriteLine("Reading recent transactions...");
            PrintRecentTransactions(_transactionManager.DownloadTransactionHistory(DateTime.Today.AddDays(-30), DateTime.Today).ToList());
            PrintSeparator();
        }

        private void PrintSeparator()
        {
            Console.WriteLine();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine();
        }

        private void PrintAccountSummaries(IReadOnlyCollection<IAccountSummary> accountSummaries)
        {
            Console.WriteLine("Found {0} accounts!", accountSummaries.Count);
            foreach (var account in accountSummaries)
            {
                Console.WriteLine("Account Name: {0}", account.Name);
                Console.WriteLine("Account Number: {0}", account.AccountNumber);
                Console.WriteLine("Account Type: {0}", account.AccountType);
                Console.WriteLine("Account Value: {0:C}", account.MostRecentValue);
                Console.WriteLine();
            }
        }

        private void PrintAccountDetails(IReadOnlyCollection<IAccountDetails> accountDetails)
        {
            Console.WriteLine($"Found {accountDetails.Count} accounts!");
            foreach (var accountDetail in accountDetails)
            {
                Console.WriteLine("Account Name: {0}", accountDetail.Name);
                Console.WriteLine("Account Number: {0}", accountDetail.AccountNumber);
                Console.WriteLine("Account Type: {0}", accountDetail.AccountType);
                Console.WriteLine("Found {0} positions in this account!", accountDetail.Positions.Count());
                foreach (var position in accountDetail.Positions)
                {
                    Console.WriteLine("Ticker: {0}", position.Ticker);
                    Console.WriteLine("Shares: {0:N}", position.Quantity);
                    Console.WriteLine("Current value: {0:C}", position.CurrentValue);
                    Console.WriteLine("Cost basis: {0:C}", position.CostBasisPerShare);
                    Console.WriteLine();
                }
            }
        }

        private void PrintRecentTransactions(IReadOnlyCollection<IFidelityTransaction> transactions)
        {
            Console.WriteLine("Found {0} recent transactions!", transactions.Count);
            foreach (var transaction in transactions)
            {
                Console.WriteLine("On {0:d} {1:F} shares of {2} were {3} at {4:C} per share",
                    transaction.RunDate, transaction.Quantity, transaction.Symbol,
                    _transactionTranslator.Translate(transaction.Type), transaction.Price);
            }
            Console.WriteLine();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _positionsManager?.Dispose();
                _positionsManager = null;

                _transactionManager?.Dispose();
                _transactionManager = null;
            }
        }
    }
}