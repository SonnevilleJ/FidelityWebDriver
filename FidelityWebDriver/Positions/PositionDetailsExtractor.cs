using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Positions.DetailExtractors;

namespace Sonneville.FidelityWebDriver.Positions
{
    public class PositionDetailsExtractor : IPositionDetailsExtractor
    {
        private readonly IPositionCoreExtractor _positionCoreExtractor;
        private readonly IPositionTickerExtractor _positionTickerExtractor;
        private readonly IPositionLastPriceExtractor _positionLastPriceExtractor;
        private readonly IPositionTotalGainExtractor _positionTotalGainExtractor;
        private readonly IPositionCurrentValueExtractor _positionCurrentValueExtractor;

        public PositionDetailsExtractor(IPositionCoreExtractor positionCoreExtractor,
            IPositionTickerExtractor positionTickerExtractor, IPositionLastPriceExtractor positionLastPriceExtractor,
            IPositionTotalGainExtractor positionTotalGainExtractor, IPositionCurrentValueExtractor positionCurrentValueExtractor)
        {
            _positionCoreExtractor = positionCoreExtractor;
            _positionTickerExtractor = positionTickerExtractor;
            _positionLastPriceExtractor = positionLastPriceExtractor;
            _positionTotalGainExtractor = positionTotalGainExtractor;
            _positionCurrentValueExtractor = positionCurrentValueExtractor;
        }

        public IEnumerable<IPosition> ExtractPositionDetails(IEnumerable<IWebElement> positionTableRows)
        {
            foreach (
                var positionTableRow in positionTableRows.Where(row => row.GetAttribute("class").Contains("normal-row"))
                )
            {
                var tdElements = positionTableRow.FindElements(By.XPath("./td"));
                var position = new Position();

                position.IsCore = _positionCoreExtractor.ExtractIsCore(tdElements);
                position.Ticker = _positionCoreExtractor.ExtractIsCore(tdElements)
                    ? _positionTickerExtractor.ExtractCoreTicker(tdElements)
                    : _positionTickerExtractor.ExtractNonCoreTicker(tdElements);
                position.Description = _positionTickerExtractor.ExtractDescription(tdElements);
                position.LastPrice = _positionLastPriceExtractor.ExtractLastPrice(tdElements);
                position.TotalGainDollar = _positionTotalGainExtractor.ExtractTotalGainDollar(tdElements);
                position.TotalGainPercent = _positionTotalGainExtractor.ExtractTotalGainPercent(tdElements);
                position.CurrentValue = _positionCurrentValueExtractor.ExtractCurrentValue(tdElements);
                position.Quantity = ExtractQuantity(tdElements);
                position.IsMargin = ExtractMargin(tdElements);
                position.CostBasisPerShare = ExtractCostBasisPerShare(tdElements);
                position.CostBasisTotal = ExtractCostBasisTotal(tdElements);

                yield return position;
            }
        }

        private decimal ExtractQuantity(IReadOnlyList<IWebElement> tdElements)
        {
            var rawText = tdElements[5].Text;
            var quantityText = ExtractMargin(tdElements)
                ? rawText.Replace("<br>", "").Replace("(Margin)", "").Replace("\"", "")
                : rawText;
            return decimal.Parse(quantityText.Trim(), NumberStyles.Any);
        }

        private bool ExtractMargin(IReadOnlyList<IWebElement> tdElements)
        {
            return tdElements[5].Text.Contains("(Margin)");
        }

        private decimal ExtractCostBasisPerShare(IReadOnlyList<IWebElement> tdElements)
        {
            var spanText = tdElements[6].FindElements(By.ClassName("magicgrid--stacked-data-value"))[0].Text;
            return spanText == "n/a" //TODO|| string.IsNullOrWhiteSpace(spanText)
                ? 0
                : decimal.Parse(spanText.Replace("/Share", ""), NumberStyles.Any);
        }

        private decimal ExtractCostBasisTotal(IReadOnlyList<IWebElement> tdElements)
        {
            var spanText = tdElements[6]
                .FindElements(By.ClassName("magicgrid--stacked-data-value"))[1]
                .FindElement(By.TagName("span"))
                .Text;
            return spanText == "n/a" //TODO|| string.IsNullOrWhiteSpace(spanText)
                ? 0
                : decimal.Parse(spanText, NumberStyles.Any);
        }
    }
}