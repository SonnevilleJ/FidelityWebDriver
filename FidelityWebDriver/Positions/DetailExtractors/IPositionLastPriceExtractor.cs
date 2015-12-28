using System.Collections.Generic;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Positions.DetailExtractors
{
    public interface IPositionLastPriceExtractor
    {
        decimal ExtractLastPrice(IReadOnlyList<IWebElement> tdElements);
    }
}