using System.Collections.Generic;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Positions
{
    public interface IAccountDetailsExtractor
    {
        IEnumerable<IAccountDetails> ExtractAccountDetails(IWebDriver webDriver);
    }
}