using System;

namespace Sonneville.FidelityWebDriver.Pages
{
    public interface IActivityPage : IPage
    {
        string DownloadHistory(DateTime minValue, DateTime maxValue);
    }
}