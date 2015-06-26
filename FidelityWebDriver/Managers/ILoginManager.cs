using System;

namespace Sonneville.FidelityWebDriver.Managers
{
    public interface ILoginManager : IManager
    {
        void EnsureLoggedIn();
    }
}