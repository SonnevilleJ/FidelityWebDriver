using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Navigation;
using Sonneville.Utilities;

namespace Sonneville.FidelityWebDriver.Positions
{
    public class PositionsPage : IPositionsPage
    {
        private readonly IWebDriver _webDriver;
        private readonly IPageFactory _pageFactory;
        private readonly IAccountSummariesExtractor _accountSummariesExtractor;
        private readonly IAccountDetailsExtractor _accountDetailsExtractor;
        private readonly ISleepUtil _sleepUtil;

        public PositionsPage(IWebDriver webDriver,
            IPageFactory pageFactory,
            IAccountSummariesExtractor accountSummariesExtractor,
            IAccountDetailsExtractor accountDetailsExtractor,
            ISleepUtil sleepUtil)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;
            _accountSummariesExtractor = accountSummariesExtractor;
            _accountDetailsExtractor = accountDetailsExtractor;
            _sleepUtil = sleepUtil;
        }

        public IEnumerable<IAccountSummary> GetAccountSummaries()
        {
            return _accountSummariesExtractor.ExtractAccountSummaries(_webDriver);
        }

        public IEnumerable<IAccountDetails> GetAccountDetails()
        {
            _sleepUtil.Sleep(1000);
            return _accountDetailsExtractor.ExtractAccountDetails(_webDriver);
        }
    }
}