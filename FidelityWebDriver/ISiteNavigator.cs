using System;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver
{
    public interface ISiteNavigator : IDisposable
    {
        IHomePage GoToHomepage();
        ILoginPage GoToLoginPage();
        IActivityPage GoToActivityPage();
    }
}