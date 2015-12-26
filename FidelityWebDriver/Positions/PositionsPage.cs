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
        private readonly IAccountDetailsExtractor _accountDetailsExtractor;

        public PositionsPage(IWebDriver webDriver, IPageFactory pageFactory,
            IAccountSummariesExtractor accountSummariesExtractor, IAccountDetailsExtractor accountDetailsExtractor)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;
            _accountSummariesExtractor = accountSummariesExtractor;
            _accountDetailsExtractor = accountDetailsExtractor;
        }

        public IEnumerable<IAccountSummary> GetAccountSummaries()
        {
            return _accountSummariesExtractor.ExtractAccountSummaries(_webDriver);
        }

        public IEnumerable<IAccountDetails> GetAccountDetails()
        {
            return _accountDetailsExtractor.ExtractAccountDetails(_webDriver);
        }
    }
}