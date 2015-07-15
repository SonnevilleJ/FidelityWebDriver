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

        [SetUp]
        public void Setup()
        {
            _startingValues = new List<Dictionary<string, string>>
            {
                CreateAccountValues("abc1234", "IA", "my taxable investment account", "123.45678"),
                CreateAccountValues("retirement101", "RA", "my 401(k) account", "98765.43211"),
            };

            _accountDivs = _startingValues
                .Select(CreateAccountDivMock)
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
            var accounts = _positionsPage.BuildAccounts();

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

        private static IWebElement CreateAccountDivMock(IReadOnlyDictionary<string, string> values)
        {
            var accountDivMock = new Mock<IWebElement>();
            accountDivMock.Setup(div => div.GetAttribute("data-acct-number")).Returns(values["accountNumber"]);
            accountDivMock.Setup(div => div.GetAttribute("data-group-id")).Returns(values["accountType"]);
            accountDivMock.Setup(div => div.GetAttribute("data-account-name")).Returns(values["accountName"]);
            accountDivMock.Setup(div => div.GetAttribute("data-most-recent-value")).Returns(values["accountValue"]);
            return accountDivMock.Object;
        }
    }
}