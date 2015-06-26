using System;

namespace Sonneville.FidelityWebDriver.Demo
{
    public interface IApp : IDisposable
    {
        void Run(string[] args);
    }
}