using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using log4net;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Demo.log4net
{
    public class LoggingWebElement : IWebElement
    {
        private readonly IWebElement _webElementImplementation;

        public LoggingWebElement(IWebElement webElementImplementation)
        {
            _webElementImplementation = webElementImplementation;
        }

        public IWebElement FindElement(By by)
        {
            LogTrace($"Finding element by {by}.");
            var element = _webElementImplementation.FindElement(by);
            return new LoggingWebElement(element);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            LogTrace($"Finding elements by {by}.");
            var elements = _webElementImplementation.FindElements(by);
            return elements.Select(e => new LoggingWebElement(e) as IWebElement).ToList().AsReadOnly();
        }

        public void Clear()
        {
            LogTrace($"Clearing {_webElementImplementation}");
            _webElementImplementation.Clear();
        }

        public void SendKeys(string text)
        {
            LogVerbose($"Sending keys: \"{text}\".");
            _webElementImplementation.SendKeys(text);
        }

        public void Submit()
        {
            LogTrace($"Submitting {_webElementImplementation}");
            _webElementImplementation.Submit();
        }

        public void Click()
        {
            LogTrace($"Clicking {_webElementImplementation}");
            _webElementImplementation.Click();
        }

        public string GetAttribute(string attributeName)
        {
            var attribute = _webElementImplementation.GetAttribute(attributeName);
            LogTrace($"Got attribute {attributeName} for {_webElementImplementation}: {attribute}");
            return attribute;
        }

        public string GetCssValue(string propertyName)
        {
            var cssValue = _webElementImplementation.GetCssValue(propertyName);
            LogTrace($"Got CSS value {propertyName} for {_webElementImplementation}: {cssValue}");
            return cssValue;
        }

        public string TagName => _webElementImplementation.TagName;

        public string Text => _webElementImplementation.Text;

        public bool Enabled => _webElementImplementation.Enabled;

        public bool Selected => _webElementImplementation.Selected;

        public Point Location => _webElementImplementation.Location;

        public Size Size => _webElementImplementation.Size;

        public bool Displayed => _webElementImplementation.Displayed;

        private static void LogTrace(string message)
        {
            var declaringType = new StackTrace(2, false)
                .GetFrame(0)
                .GetMethod()
                .DeclaringType;
            LogManager.GetLogger(declaringType).Trace(message, declaringType);
        }

        private static void LogVerbose(string message)
        {
            var declaringType = new StackTrace(2, false)
                .GetFrame(0)
                .GetMethod()
                .DeclaringType;
            LogManager.GetLogger(declaringType).Verbose(message, declaringType);
        }
    }
}