using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Positions;

namespace Sonneville.FidelityWebDriver.Tests.Positions
{
    [TestFixture]
    public class PositionDetailsExtractorTests
    {
        private PositionDetailsExtractor _extractor;
        private List<IWebElement> _positionTableRows;
        private List<IPosition> _positions;

        [SetUp]
        public void Setup()
        {
            _positions = SetupPositions();

            _positionTableRows = SetupPositionRows(_positions);

            _extractor = new PositionDetailsExtractor();
        }

        [Test]
        public void ShouldExtractPositionDetails()
        {
            var actuals = _extractor.ExtractPositionDetails(_positionTableRows).ToList();

            Assert.AreEqual(_positions.Count(), actuals.Count());
            foreach (var actual in actuals)
            {
                var matchingExpected = _positions.Single(expected => expected.Ticker == actual.Ticker
                    /* && expected.IsMargin == actual.IsMargin */);

                Assert.AreEqual(matchingExpected.Description, actual.Description);
                Assert.AreEqual(matchingExpected.IsCore, actual.IsCore);
                Assert.AreEqual(matchingExpected.LastPrice, actual.LastPrice);
                Assert.AreEqual(matchingExpected.TotalGainDollar, actual.TotalGainDollar);
                Assert.AreEqual(matchingExpected.TotalGainPercent, actual.TotalGainPercent);
                Assert.AreEqual(matchingExpected.CurrentValue, actual.CurrentValue);
                Assert.AreEqual(matchingExpected.Quantity, actual.Quantity);
                Assert.AreEqual(matchingExpected.CostBasisPerShare, actual.CostBasisPerShare);
                Assert.AreEqual(matchingExpected.CostBasisTotal, actual.CostBasisTotal);
            }
        }

        private List<IPosition> SetupPositions()
        {
            return new List<IPosition>
            {
                new Position("a", "b", true, false, 1.23m, 0, 0, 901.23m, 45.678m, 0, 0),
                new Position("c", "d", false, false, 3.21m, 2.34m, 0.56m, 246m, 45.678m, 1.23m, 123456.78m),
            };
        }

        private List<IWebElement> SetupPositionRows(IEnumerable<IPosition> positions)
        {
            return positions.Select(CreatePositionRow).ToList();
        }

        private IWebElement CreatePositionRow(IPosition position)
        {
            var tdMocks = new List<IWebElement>
            {
                SetupSymbolTd(position),
                SetupLastPriceTd(position),
                SetupTodaysGainTd(position),
                SetupTotalGainTd(position),
                SetupCurrentValueTd(position),
                SetupQuantityTd(position),
                SetupCostBasisTd(position),
            };

            var rowMock = new Mock<IWebElement>();
            rowMock.Setup(row => row.FindElements(By.XPath("./td"))).Returns(tdMocks.AsReadOnly());

            return rowMock.Object;
        }

        private IWebElement SetupSymbolTd(IPosition position)
        {
            var ticker = position.IsCore
                ? $"{position.Ticker}**"
                : position.Ticker;

            var symbolSpanMock = new Mock<IWebElement>();
            symbolSpanMock.Setup(span => span.Text).Returns(ticker);

            var descriptionSpanMock = new Mock<IWebElement>();
            descriptionSpanMock.Setup(span => span.Text).Returns(position.Description);

            var tdMock = new Mock<IWebElement>();
            tdMock.Setup(td => td.FindElement(By.ClassName("stock-symbol"))).Returns(symbolSpanMock.Object);
            tdMock.Setup(td => td.FindElement(By.ClassName("stock-name"))).Returns(descriptionSpanMock.Object);
            return tdMock.Object;
        }

        private IWebElement SetupLastPriceTd(IPosition position)
        {
            var spanMock = new Mock<IWebElement>();
            spanMock.Setup(span => span.Text).Returns(position.LastPrice.ToString("C"));

            var tdMock = new Mock<IWebElement>();
            tdMock.Setup(td => td.FindElements(By.ClassName("magicgrid--stacked-data-value")))
                .Returns(new List<IWebElement> {spanMock.Object}.AsReadOnly());
            return tdMock.Object;
        }

        private IWebElement SetupTodaysGainTd(IPosition position)
        {
            return new Mock<IWebElement>().Object;
        }

        private IWebElement SetupTotalGainTd(IPosition position)
        {
            var totalGainDollarString = position.TotalGainDollar != 0
                ? position.TotalGainDollar.ToString("C")
                : "n/a";

            var span1 = new Mock<IWebElement>();
            span1.Setup(span => span.Text).Returns(totalGainDollarString);

            var totalGainPercentString = position.TotalGainPercent != 0
                ? position.TotalGainPercent.ToString("P")
                : "n/a";
            var span2 = new Mock<IWebElement>();
            span2.Setup(span => span.Text).Returns(totalGainPercentString);

            var tdMock = new Mock<IWebElement>();
            tdMock.Setup(td => td.FindElements(By.ClassName("magicgrid--stacked-data-value")))
                .Returns(new List<IWebElement> {span1.Object, span2.Object}.AsReadOnly());
            return tdMock.Object;
        }

        private IWebElement SetupCurrentValueTd(IPosition position)
        {
            var tdMock = new Mock<IWebElement>();
            tdMock.Setup(td => td.Text).Returns(position.CurrentValue.ToString("C"));
            return tdMock.Object;
        }

        private IWebElement SetupQuantityTd(IPosition position)
        {
            var tdMock = new Mock<IWebElement>();
            tdMock.Setup(td => td.Text).Returns(position.Quantity.ToString("##.###"));
            return tdMock.Object;
        }

        private IWebElement SetupCostBasisTd(IPosition position)
        {
            var costBasisPerShareString = position.CostBasisPerShare != 0
                ? $"{position.CostBasisPerShare.ToString("C")}/Share"
                : "n/a";
            var perShareSpanMock = new Mock<IWebElement>();
            perShareSpanMock.Setup(span => span.Text).Returns(costBasisPerShareString);

            var totalSpanMock = new Mock<IWebElement>();
            var costBasisTotalString = position.CostBasisTotal != 0
                ? $"{position.CostBasisTotal.ToString("C")}"
                : "n/a";
            totalSpanMock.Setup(span => span.Text).Returns(costBasisTotalString);

            var totalDivMock = new Mock<IWebElement>();
            totalDivMock.Setup(div => div.FindElement(By.TagName("span"))).Returns(totalSpanMock.Object);

            var tdMock = new Mock<IWebElement>();
            tdMock.Setup(td => td.FindElements(By.ClassName("magicgrid--stacked-data-value")))
                .Returns(new List<IWebElement> { perShareSpanMock.Object, totalDivMock.Object }.AsReadOnly());

            return tdMock.Object;
        }
    }
}