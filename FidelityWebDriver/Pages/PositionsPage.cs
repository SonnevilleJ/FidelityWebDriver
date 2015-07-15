using System.Collections.Generic;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Pages
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
            var accountDivs = _webDriver.FindElements(By.ClassName("account-selector--group-container"));

            foreach (var accountDiv in accountDivs)
            {
                var accountNumber = accountDiv.GetAttribute("data-acct-number");
                var accountType = _groupIds[accountDiv.GetAttribute("data-group-id")];
                var accountName = accountDiv.GetAttribute("data-account-name");
                var value = double.Parse(accountDiv.GetAttribute("data-most-recent-value"));

                yield return new Account(accountNumber, accountType, accountName, value);
            }
        }
    }
}