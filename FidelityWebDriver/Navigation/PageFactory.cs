using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Login;
using Sonneville.FidelityWebDriver.Positions;
using Sonneville.FidelityWebDriver.Transactions;
using Sonneville.Utilities;

namespace Sonneville.FidelityWebDriver.Navigation
{
    public class PageFactory : IPageFactory
    {
        private readonly Dictionary<Type, IPage> _pages;

        public PageFactory(IWebDriver webDriver,
            IAccountSummariesExtractor accountSummariesExtractor,
            IAccountDetailsExtractor accountDetailsExtractor,
            ISleepUtil sleepUtil,
            IHistoryTransactionParser historyTransactionParser)
        {
            _pages = new Dictionary<Type, IPage>
            {
                {
                    typeof(IHomePage),
                    new HomePage(webDriver)
                },
                {
                    typeof(ILoginPage),
                    new LoginPage(webDriver)
                },
                {
                    typeof(ISummaryPage),
                    new SummaryPage(webDriver)
                },
                {
                    typeof(IPositionsPage),
                    new PositionsPage(webDriver, accountSummariesExtractor, accountDetailsExtractor, sleepUtil)
                },
                {
                    typeof(IActivityPage),
                    new ActivityPage(webDriver, historyTransactionParser)
                },
            };
        }

        public T GetPage<T>() where T : IPage
        {
            return (T) _pages[typeof(T)];
        }
    }
}