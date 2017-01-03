using System;
using Sonneville.FidelityWebDriver.Navigation.Pages;

namespace Sonneville.FidelityWebDriver.Navigation
{
    public interface ISiteNavigator : IDisposable
    {
        TPage GoTo<TPage>() where TPage : IPage;
    }
}