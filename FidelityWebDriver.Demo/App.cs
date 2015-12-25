using System;
using System.Collections.Generic;
using System.Linq;
using NDesk.Options;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Managers;

namespace Sonneville.FidelityWebDriver.Demo
{
    public class App : IApp
    {
        private readonly IPositionsManager _positionsManager;

        private readonly ITransactionManager _transactionManager;

        private readonly FidelityConfiguration _fidelityConfiguration;
        private readonly OptionSet _optionSet;
        private bool _shouldPersistOptions;
        private bool _shouldShowHelp;

        public App(IPositionsManager positionsManager,
            ITransactionManager transactionManager,
            FidelityConfiguration fidelityConfiguration)
        {
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
                    "s|save", "indicates options should be persisted.",
                    save => { _shouldPersistOptions = true; }
                },
                {
                    "h|help", "shows this message and exits.",
                    help => { _shouldShowHelp = true; }
                },
            };
            _fidelityConfiguration = fidelityConfiguration;
            _positionsManager = positionsManager;
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

            var accounts = _positionsManager.GetAccounts().ToList();

            Console.WriteLine("Found {0} accounts!", accounts.Count());
            foreach (var account in accounts)
            {
                Console.WriteLine("Account Name: {0}", account.Name);
                Console.WriteLine("Account Number: {0}", account.AccountNumber);
                Console.WriteLine("Account Type: {0}", account.AccountType);
                Console.WriteLine("Account Value: {0}", account.MostRecentValue.ToString("C"));
                Console.WriteLine();
            }

            var transactions = _transactionManager.DownloadTransactionHistory();
            Console.WriteLine("Found {0} recent transactions!", transactions.Count());
            foreach (var transaction in transactions)
            {
                Console.WriteLine("On {0:d} {1:F} shares of {2} were {3} at {4:C} per share",
                    transaction.RunDate, transaction.Quantity, transaction.Symbol, Translate(transaction),
                    transaction.Price);
            }
        }

        private static string Translate(IFidelityTransaction transaction)
        {
            switch (transaction.Type)
            {
                case TransactionType.Deposit:
                    return "deposited";
                case TransactionType.Withdrawal:
                    return "withdrew";
                case TransactionType.Buy:
                    return "bought";
                case TransactionType.Sell:
                    return "sold";
                case TransactionType.BuyToCover:
                    return "bought to cover short position";
                case TransactionType.SellShort:
                    return "sold short";
                case TransactionType.DividendReceipt:
                    return "received";
                case TransactionType.DividendReinvestment:
                    return "reinvested";
                default:
                    return $"Unknown transaction type: {transaction.Type}";
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var positionsManager = _positionsManager;
                positionsManager?.Dispose();
                var transactionManager = _transactionManager;
                transactionManager?.Dispose();
            }
        }
    }
}