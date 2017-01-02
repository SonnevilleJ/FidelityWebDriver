using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using log4net;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Demo.log4net
{
    public class LoggingWebDriver : IWebDriver
    {
        private readonly IWebDriver _webDriverImplementation;

        public LoggingWebDriver(IWebDriver webDriverImplementation)
        {
            _webDriverImplementation = webDriverImplementation;
        }

        public IWebElement FindElement(By by)
        {
            LogTrace($"Finding element by {by}.");
            var element = _webDriverImplementation.FindElement(by);
            return new LoggingWebElement(element);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            LogTrace($"Finding elements by {by}.");
            var elements = _webDriverImplementation.FindElements(by);
            return elements.Select(e => new LoggingWebElement(e) as IWebElement).ToList().AsReadOnly();
        }

        public void Dispose()
        {
            LogTrace($"Disposing web driver {_webDriverImplementation}");
            _webDriverImplementation?.Dispose();
        }

        public void Close()
        {
            LogTrace($"Closing web driver {_webDriverImplementation}");
            _webDriverImplementation.Close();
        }

        public void Quit()
        {
            LogTrace($"Disposing web driver {_webDriverImplementation}");
            _webDriverImplementation.Quit();
        }

        public IOptions Manage()
        {
            return _webDriverImplementation.Manage();
        }

        public INavigation Navigate()
        {
            return _webDriverImplementation.Navigate();
        }

        public ITargetLocator SwitchTo()
        {
            return _webDriverImplementation.SwitchTo();
        }

        public string Url
        {
            get { return _webDriverImplementation.Url; }
            set
            {
                LogTrace($"Setting web driver URL: {value}");
                _webDriverImplementation.Url = value;
            }
        }

        public string Title => _webDriverImplementation.Title;

        public string PageSource => _webDriverImplementation.PageSource;

        public string CurrentWindowHandle => _webDriverImplementation.CurrentWindowHandle;

        public ReadOnlyCollection<string> WindowHandles => _webDriverImplementation.WindowHandles;

        private static void LogTrace(string message)
        {
            var declaringType = new StackTrace(2, false)
                .GetFrame(0)
                .GetMethod()
                .DeclaringType;
            LogManager.GetLogger(declaringType).Trace(message, declaringType);
        }
    }
}
