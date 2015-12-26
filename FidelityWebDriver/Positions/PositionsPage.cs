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
        private readonly IAccountSummariesExtractor _accountSummariesExtractor;

        public PositionsPage(IWebDriver webDriver, IPageFactory pageFactory,
            IAccountSummariesExtractor accountSummariesExtractor)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;
            _accountSummariesExtractor = accountSummariesExtractor;
        }

        public IEnumerable<IAccount> GetAccountSummaries()
        {
            return _accountSummariesExtractor.ExtractAccountSummaries(_webDriver);
        }
    }
}