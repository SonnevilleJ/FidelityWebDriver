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
        private readonly IPositionsPageAccountsExtractor _positionsPageAccountsExtractor;

        public PositionsPage(IWebDriver webDriver, IPageFactory pageFactory,
            IPositionsPageAccountsExtractor positionsPageAccountsExtractor)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;
            _positionsPageAccountsExtractor = positionsPageAccountsExtractor;
        }

        public IEnumerable<IAccount> GetAccountSummaries()
        {
            return _positionsPageAccountsExtractor.ExtractAccountSummaries(_webDriver);
        }
    }
}