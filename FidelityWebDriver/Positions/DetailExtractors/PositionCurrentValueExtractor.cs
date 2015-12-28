using System.Collections.Generic;
using System.Globalization;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Positions.DetailExtractors
{
    public class PositionCurrentValueExtractor : IPositionCurrentValueExtractor
    {
        public decimal ExtractCurrentValue(IReadOnlyList<IWebElement> tdElements)
        {
            return decimal.Parse(tdElements[4].Text, NumberStyles.Any);
        }
    }
}