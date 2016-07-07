using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Login;
using Sonneville.FidelityWebDriver.Positions;
using Sonneville.FidelityWebDriver.Transactions;
using Sonneville.FidelityWebDriver.Transactions.CSV;
using Sonneville.Utilities;

namespace Sonneville.FidelityWebDriver.Navigation
{
    public class PageFactory : IPageFactory
    {
        private readonly Dictionary<Type, IPage> _pages;

        public PageFactory(IWebDriver webDriver,
            ICsvDownloadService csvDownloadService,
            IAccountSummariesExtractor accountSummariesExtractor,
            IAccountDetailsExtractor accountDetailsExtractor,
            ISleepUtil sleepUtil)
        {
            _pages = new Dictionary<Type, IPage>
            {
                {typeof(IHomePage), new HomePage(webDriver, this)},
                {typeof(ILoginPage), new LoginPage(webDriver, this)},
                {typeof(ISummaryPage), new SummaryPage(webDriver, this)},
                {
                    typeof(IPositionsPage),
                    new PositionsPage(webDriver, this, accountSummariesExtractor, accountDetailsExtractor, sleepUtil)
                },
                {typeof(IActivityPage), new ActivityPage(webDriver, this, csvDownloadService, sleepUtil)},
            };
        }

        public T GetPage<T>() where T : IPage
        {
            return (T) _pages[typeof(T)];
        }
    }
}