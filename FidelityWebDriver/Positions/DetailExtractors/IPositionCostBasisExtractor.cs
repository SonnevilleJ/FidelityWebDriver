using System.Collections.Generic;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Positions.DetailExtractors
{
    public interface IPositionCostBasisExtractor
    {
        decimal ExtractCostBasisPerShare(IReadOnlyList<IWebElement> tdElements);

        decimal ExtractCostBasisTotal(IReadOnlyList<IWebElement> tdElements);
    }
}