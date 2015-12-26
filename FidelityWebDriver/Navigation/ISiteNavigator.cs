using System;

namespace Sonneville.FidelityWebDriver.Navigation
{
    public interface ISiteNavigator : IDisposable
    {
        TPage GoTo<TPage>() where TPage : IPage;
    }
}