using System.Collections.Generic;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Positions.DetailExtractors
{
    public class PositionCoreExtractor : IPositionCoreExtractor
    {
        public bool ExtractIsCore(IReadOnlyList<IWebElement> tdElements)
        {
            return tdElements[0].FindElement(By.ClassName("stock-symbol")).Text.Contains("**");
        }
    }
}