using System.Collections.Generic;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Positions
{
    public interface IPositionCoreExtractor
    {
        bool ExtractIsCore(IReadOnlyList<IWebElement> tdElements);
    }
}