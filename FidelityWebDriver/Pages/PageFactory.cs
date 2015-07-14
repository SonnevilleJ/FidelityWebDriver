using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Pages
{
    public class PageFactory : IPageFactory
    {
        private readonly Dictionary<Type, IPage> _pages;

        public PageFactory(IWebDriver webDriver)
        {
            _pages = new Dictionary<Type, IPage>
            {
                {typeof (IHomePage), new HomePage(webDriver, this)},
                {typeof (ILoginPage), new LoginPage(webDriver, this)},
                {typeof (ISummaryPage), new SummaryPage(webDriver, this)},
                {typeof(IPositionsPage), new PositionsPage(webDriver, this)},
            };
        }

        public T GetPage<T>() where T : IPage
        {
            return (T) _pages[typeof (T)];
        }
    }
}