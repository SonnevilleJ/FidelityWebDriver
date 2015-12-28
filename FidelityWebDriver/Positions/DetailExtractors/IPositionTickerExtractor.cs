using System.Collections.Generic;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Positions.DetailExtractors
{
    public interface IPositionTickerExtractor
    {
        string ExtractCoreTicker(IReadOnlyList<IWebElement> tdElements);

        string ExtractNonCoreTicker(IReadOnlyList<IWebElement> tdElements);

        string ExtractDescription(IReadOnlyList<IWebElement> tdElements);
    }
}