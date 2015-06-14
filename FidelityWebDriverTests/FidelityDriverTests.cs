﻿using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver;

namespace Sonneville.FidelityWebDriverTests
{
    [TestFixture]
    public class FidelityDriverTests
    {
        private FidelityDriver _fidelityDriver;
        private Mock<IWebDriver> _webDriverMock;
        private Mock<INavigation> _navigationMock;

        [SetUp]
        public void Setup()
        {
            _navigationMock = new Mock<INavigation>();

            _webDriverMock = new Mock<IWebDriver>();
            _webDriverMock.Setup(webDriver => webDriver.Navigate()).Returns(_navigationMock.Object);

            _fidelityDriver = new FidelityDriver(_webDriverMock.Object);
        }

        [Test]
        public void ShouldOpenFidelitySite()
        {
            _fidelityDriver.GoToHomepage();

            _navigationMock.Verify(navigation => navigation.GoToUrl("https://www.fidelity.com"));
        }

        [Test]
        public void ShouldDisposeWebDriver()
        {
            _fidelityDriver.Dispose();

            _webDriverMock.Verify(driver => driver.Dispose());
        }
    }
}