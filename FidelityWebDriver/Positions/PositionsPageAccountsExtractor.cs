using System.Collections.Generic;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Positions
{
    public class PositionsPageAccountsExtractor : IPositionsPageAccountsExtractor
    {
        private readonly Dictionary<string, AccountType> _groupIds;

        public PositionsPageAccountsExtractor()
        {
            _groupIds = new Dictionary<string, AccountType>
            {
                {"IA", AccountType.InvestmentAccount},
                {"RA", AccountType.RetirementAccount},
                {"HS", AccountType.HealthSavingsAccount},
                {"OA", AccountType.Other},
                {"CC", AccountType.CreditCard},
            };
        }

        public IEnumerable<IAccount> ExtractAccountSummaries(IWebDriver webDriver)
        {
            var accountGroupDivs = webDriver.FindElements(By.ClassName("account-selector--group-container"));

            foreach (var accountGroupDiv in accountGroupDivs)
            {
                var accountType = _groupIds[accountGroupDiv.GetAttribute("data-group-id")];
                var accountDivs = accountGroupDiv.FindElements(By.ClassName("js-account"));

                foreach (var accountDiv in accountDivs)
                {
                    var accountNumber = accountDiv.GetAttribute("data-acct-number");
                    var accountName = accountDiv.GetAttribute("data-acct-name");
                    var value = accountType == AccountType.CreditCard
                        ? double.Parse(accountDiv.GetAttribute("data-acct-balance"))
                        : double.Parse(accountDiv.GetAttribute("data-most-recent-value"));
                    yield return new Account(accountNumber, accountType, accountName, value);
                }
            }
        }
    }
}