using System;
using System.Collections.Generic;

namespace Sonneville.FidelityWebDriver.Demo
{
    public interface IApp : IDisposable
    {
        void Run(IEnumerable<string> args);
    }
}