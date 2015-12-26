using System.Collections.Generic;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Positions
{
    public class PositionsPage : IPositionsPage
    {
        private readonly IWebDriver _webDriver;
        private readonly IPageFactory _pageFactory;
        private readonly IDictionary<string, AccountType> _groupIds;

        public PositionsPage(IWebDriver webDriver, IPageFactory pageFactory)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;

            _groupIds = new Dictionary<string, AccountType>
            {
                {"IA", AccountType.InvestmentAccount},
                {"RA", AccountType.RetirementAccount},
                {"HS", AccountType.HealthSavingsAccount},
                {"OA", AccountType.Other},
                {"CC", AccountType.CreditCard},
            };
        }

        public IEnumerable<IAccount> BuildAccounts()
        {
            var accountGroupDivs = _webDriver.FindElements(By.ClassName("account-selector--group-container"));

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