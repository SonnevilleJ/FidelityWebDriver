using System;

namespace Sonneville.FidelityWebDriver
{
    public interface IFidelityDriver : IDisposable
    {
        void GoToHomepage();
    }
}