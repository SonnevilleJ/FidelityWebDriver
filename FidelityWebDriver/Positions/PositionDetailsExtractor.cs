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

        public PositionDetailsExtractor(IPositionCoreExtractor positionCoreExtractor,
            IPositionTickerExtractor positionTickerExtractor, IPositionLastPriceExtractor positionLastPriceExtractor)
        {
            _positionCoreExtractor = positionCoreExtractor;
            _positionTickerExtractor = positionTickerExtractor;
            _positionLastPriceExtractor = positionLastPriceExtractor;
        }

        public IEnumerable<IPosition> ExtractPositionDetails(IEnumerable<IWebElement> positionTableRows)
        {
            foreach (var positionTableRow in positionTableRows.Where(row=>row.GetAttribute("class").Contains("normal-row")))
            {
                var tdElements = positionTableRow.FindElements(By.XPath("./td"));
                var position = new Position();

                position.IsCore = _positionCoreExtractor.ExtractIsCore(tdElements);
                position.Ticker = _positionCoreExtractor.ExtractIsCore(tdElements)
                    ? _positionTickerExtractor.ExtractCoreTicker(tdElements)
                    : _positionTickerExtractor.ExtractNonCoreTicker(tdElements);
                position.Description = _positionTickerExtractor.ExtractDescription(tdElements);
                position.LastPrice = _positionLastPriceExtractor.ExtractLastPrice(tdElements);
                position.TotalGainDollar = ExtractTotalGainDollar(tdElements);
                position.TotalGainPercent = ExtractTotalGainPercent(tdElements);
                position.CurrentValue = ExtractCurrentValue(tdElements);
                position.Quantity = ExtractQuantity(tdElements);
                position.IsMargin = ExtractMargin(tdElements);
                position.CostBasisPerShare = ExtractCostBasisPerShare(tdElements);
                position.CostBasisTotal = ExtractCostBasisTotal(tdElements);

                yield return position;
            }
        }

        private decimal ExtractTotalGainDollar(IReadOnlyList<IWebElement> tdElements)
        {
            var totalGainDollarText = tdElements[3]
                .FindElements(By.ClassName("magicgrid--stacked-data-value"))[0]
                .Text;
            return totalGainDollarText == "n/a"
                ? 0
                : decimal.Parse(totalGainDollarText, NumberStyles.Any);
        }

        private decimal ExtractTotalGainPercent(IReadOnlyList<IWebElement> tdElements)
        {
            var totalGainPercentText = tdElements[3]
                .FindElements(By.ClassName("magicgrid--stacked-data-value"))[1]
                .Text.TrimEnd('%');
            return totalGainPercentText == "n/a"
                ? 0
                : decimal.Parse(totalGainPercentText, NumberStyles.Any)/100m;
        }

        private decimal ExtractCurrentValue(IReadOnlyList<IWebElement> tdElements)
        {
            return decimal.Parse(tdElements[4].Text, NumberStyles.Any);
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