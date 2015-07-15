using System.Collections.Generic;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Pages
{
    public class PositionsPage : IPositionsPage
    {
        private readonly IWebDriver _webDriver;
        private readonly IPageFactory _pageFactory;

        public PositionsPage(IWebDriver webDriver, IPageFactory pageFactory)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;
        }

        public IEnumerable<IAccount> BuildAccounts()
        {
            throw new System.NotImplementedException();
        }
    }
}