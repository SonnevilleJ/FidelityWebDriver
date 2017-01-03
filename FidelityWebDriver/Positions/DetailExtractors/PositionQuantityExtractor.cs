using System.Collections.Generic;
using System.Globalization;
using OpenQA.Selenium;

namespace Sonneville.FidelityWebDriver.Positions.DetailExtractors
{
    public interface IPositionQuantityExtractor
    {
        decimal ExtractQuantity(IReadOnlyList<IWebElement> tdElements);

        bool ExtractMargin(IReadOnlyList<IWebElement> tdElements);
    }

    public class PositionQuantityExtractor : IPositionQuantityExtractor
    {
        public decimal ExtractQuantity(IReadOnlyList<IWebElement> tdElements)
        {
            var rawText = tdElements[5].Text;
            var quantityText = ExtractMargin(tdElements)
                ? rawText.Replace("<br>", "").Replace("(Margin)", "").Replace("\"", "")
                : rawText;
            return decimal.Parse(quantityText.Trim(), NumberStyles.Any);
        }

        public bool ExtractMargin(IReadOnlyList<IWebElement> tdElements)
        {
            return tdElements[5].Text.Contains("(Margin)");
        }
    }
}