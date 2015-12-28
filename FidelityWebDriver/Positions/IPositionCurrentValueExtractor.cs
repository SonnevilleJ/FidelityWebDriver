using System.Collections.Generic;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Positions
{
    public interface IPositionCurrentValueExtractor
    {
        decimal ExtractCurrentValue(IReadOnlyList<IWebElement> tdElements);
    }
}