using System.Collections.Generic;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Positions.DetailExtractors
{
    public interface IPositionQuantityExtractor
    {
        decimal ExtractQuantity(IReadOnlyList<IWebElement> tdElements);

        bool ExtractMargin(IReadOnlyList<IWebElement> tdElements);
    }
}