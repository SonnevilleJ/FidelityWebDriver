using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver.Tests.Pages
{
    [TestFixture]
    public class PositionsPageTests : PageFactoryTests<IPositionsPage>
    {
        private PositionsPage _positionsPage;
        private Mock<IWebDriver> _webDriverMock;
        private Mock<IPageFactory> _pageFactoryMock;
        private List<IWebElement> _accountDivs;
        private List<Dictionary<string, string>> _startingValues;
        private readonly IDictionary<string, AccountType> _accountTypeStrings = new Dictionary<string, AccountType>
        {
            {"IA", AccountType.InvestmentAccount},
            {"RA", AccountType.RetirementAccount},
            {"HS", AccountType.HealthSavingsAccount},
            {"OA", AccountType.Other},
            {"CC", AccountType.CreditCard},
        };

        private static Dictionary<string, Mock<IWebElement>> _groupDivMocks;

        [SetUp]
        public void Setup()
        {
            _startingValues = new List<Dictionary<string, string>>
            {
                CreateAccountValues("abc1234", "IA", "my taxable investment account", "123.45678"),
                CreateAccountValues("bl54321", "RA", "my BrokerageLink account", "98765.43211"),
                CreateAccountValues("hsa9876", "HS", "my health savings account", "1357.02468"),
                CreateAccountValues("401kplan", "OA", "my 401(k) account", "0"),
                CreateAccountValues("credit1", "CC", "my first credit card", "1234.56"),
                CreateAccountValues("credit2", "CC", "my second credit card", "12.34"),
            };

            _groupDivMocks = new Dictionary<string, Mock<IWebElement>>();

            _accountDivs = _startingValues
                .GroupBy(d=>d["accountType"])
                .Select(CreateAccountGroupDivMock)
                .ToList();

            _webDriverMock = new Mock<IWebDriver>();
            _webDriverMock.Setup(driver => driver.FindElements(By.ClassName("account-selector--group-container")))
                .Returns(_accountDivs.AsReadOnly());

            _pageFactoryMock = new Mock<IPageFactory>();

            _positionsPage = new PositionsPage(_webDriverMock.Object, _pageFactoryMock.Object);
        }

        [Test]
        public void ShouldReturnOneAccountPerAccountOnPage()
        {
            var accounts = _positionsPage.BuildAccounts().ToList();

            Assert.AreEqual(_startingValues.Count(), accounts.Count());
            foreach (var account in accounts)
            {
                var matchingValues = _startingValues.Single(values => values["accountNumber"] == account.AccountNumber);

                Assert.AreEqual(matchingValues["accountName"], account.Name);
                Assert.AreEqual(_accountTypeStrings[matchingValues["accountType"]], account.AccountType);
                Assert.AreEqual(double.Parse(matchingValues["accountValue"]), account.MostRecentValue);
            }
        }

        private static Dictionary<string, string> CreateAccountValues(string accountNumber, string accountType, string accountName, string accountValue)
        {
            return new Dictionary<string, string>
            {
                {"accountNumber", accountNumber},
                {"accountType", accountType},
                {"accountName", accountName},
                {"accountValue", accountValue},
            };
        }

        private IWebElement CreateAccountGroupDivMock(IGrouping<string, IReadOnlyDictionary<string, string>> accountValuesByAccountType)
        {
            var accountTypeString = accountValuesByAccountType.Key;

            var accountGroupDiv = GetOrCreateAccountGroupDiv(accountTypeString);
            accountGroupDiv.Setup(div => div.GetAttribute("data-group-id")).Returns(accountTypeString);
            var accountDivs = CreateAccountDivs(accountValuesByAccountType, accountTypeString);
            accountGroupDiv.Setup(div => div.FindElements(By.ClassName("js-account")))
                .Returns(accountDivs.ToList().AsReadOnly());
            return accountGroupDiv.Object;
        }

        private static IEnumerable<IWebElement> CreateAccountDivs(IEnumerable<IReadOnlyDictionary<string, string>> accountValues, string accountTypeString)
        {
            foreach (var values in accountValues)
            {
                var accountNumberString = values["accountNumber"];
                var accountNameString = values["accountName"];
                var accountValueString = values["accountValue"];

                var accountDivMock = CreateAccountDivMock(accountNumberString, accountNameString, accountTypeString,
                    accountValueString);
                yield return accountDivMock;
            }
        }

        private static Mock<IWebElement> GetOrCreateAccountGroupDiv(string accountTypeString)
        {
            return _groupDivMocks.ContainsKey(accountTypeString) ? _groupDivMocks[accountTypeString] : new Mock<IWebElement>();
        }

        private static IWebElement CreateAccountDivMock(string accountNumberString, string accountNameString, string accountTypeString,
            string accountValueString)
        {
            var accountDivMock = new Mock<IWebElement>();
            accountDivMock.Setup(div => div.GetAttribute("data-acct-number")).Returns(accountNumberString);
            accountDivMock.Setup(div => div.GetAttribute("data-acct-name")).Returns(accountNameString);
            switch (accountTypeString)
            {
                case "IA":
                case "RA":
                case "HS":
                case "OA":
                    accountDivMock.Setup(div => div.GetAttribute("data-most-recent-value"))
                        .Returns(accountValueString);
                    break;
                case "CC":
                    accountDivMock.Setup(div => div.GetAttribute("data-acct-balance"))
                        .Returns(accountValueString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return accountDivMock.Object;
        }
    }
}