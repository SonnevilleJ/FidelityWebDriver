using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Positions.DetailExtractors;

namespace Sonneville.FidelityWebDriver.Tests.Positions.DetailExtractors
{
    [TestFixture]
    public class PositionTickerExtractorTests
    {
        [Test]
        [TestCase(true, "asdf", "super fund")]
        [TestCase(false, "blah", "garbage fund")]
        public void Test(bool shouldBeCore, string expectedTicker, string expectedDescription)
        {
            var tdElements = new List<IWebElement> {SetupSymbolTd(shouldBeCore, expectedTicker, expectedDescription)};

            var extractor = new PositionTickerExtractor();
            var actualTicker = extractor.ExtractCoreTicker(tdElements);
            var actualDescription = extractor.ExtractDescription(tdElements);

            Assert.AreEqual(expectedTicker, actualTicker);
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        private IWebElement SetupSymbolTd(bool isCore, string ticker, string description)
        {
            var symbolSpanMock = new Mock<IWebElement>();
            symbolSpanMock.Setup(span => span.Text).Returns(isCore
                ? $"{ticker}**"
                : ticker);

            var descriptionSpanMock = new Mock<IWebElement>();
            descriptionSpanMock.Setup(span => span.Text).Returns(description);

            var tdMock = new Mock<IWebElement>();
            tdMock.Setup(td => td.FindElement(By.ClassName("stock-symbol"))).Returns(symbolSpanMock.Object);
            tdMock.Setup(td => td.FindElement(By.ClassName("stock-name"))).Returns(descriptionSpanMock.Object);
            return tdMock.Object;
        }
    }
}