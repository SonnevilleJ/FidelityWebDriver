using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Positions.DetailExtractors
{
    public interface IPositionLastPriceExtractor
    {
        decimal ExtractLastPrice(IReadOnlyList<IWebElement> tdElements);
    }

    public class PositionLastPriceExtractor : IPositionLastPriceExtractor
    {
        public decimal ExtractLastPrice(IReadOnlyList<IWebElement> tdElements)
        {
            var text = tdElements[1].FindElements(By.ClassName("magicgrid--stacked-data-value"))
                .First()
                .Text;
            return text.Contains("--")
                ? 0m
                : decimal.Parse(text, NumberStyles.Any);
        }
    }
}