using System.Collections.Generic;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Positions
{
    public interface IPositionDetailsExtractor
    {
        List<IPosition> ExtractPositionDetails(IEnumerable<IWebElement> positionTableRows);
    }
}