using System.Collections.Generic;
using System.Globalization;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Positions.DetailExtractors
{
    public class PositionTotalGainExtractor : IPositionTotalGainExtractor
    {
        public decimal ExtractTotalGainDollar(IReadOnlyList<IWebElement> tdElements)
        {
            var totalGainDollarText = tdElements[3]
                .FindElements(By.ClassName("magicgrid--stacked-data-value"))[0]
                .Text;
            return totalGainDollarText == "n/a" || totalGainDollarText == "--"
                ? 0
                : decimal.Parse(totalGainDollarText, NumberStyles.Any);
        }

        public decimal ExtractTotalGainPercent(IReadOnlyList<IWebElement> tdElements)
        {
            var totalGainPercentText = tdElements[3]
                .FindElements(By.ClassName("magicgrid--stacked-data-value"))[1]
                .Text.TrimEnd('%');
            return totalGainPercentText == "n/a" || totalGainPercentText == "--"
                ? 0
                : decimal.Parse(totalGainPercentText, NumberStyles.Any)/100m;
        }
    }
}