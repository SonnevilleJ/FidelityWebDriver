using System;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver
{
    public interface ISiteNavigator : IDisposable
    {
        TPage GoTo<TPage>() where TPage : IPage;
    }
}