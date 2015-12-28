using System.Collections.Generic;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Positions.DetailExtractors
{
    public interface IPositionTotalGainExtractor
    {
        decimal ExtractTotalGainDollar(IReadOnlyList<IWebElement> tdElements);

        decimal ExtractTotalGainPercent(IReadOnlyList<IWebElement> tdElements);
    }
}