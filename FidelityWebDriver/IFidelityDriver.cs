﻿using System;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver
{
    public interface IFidelityDriver : IDisposable
    {
        IHomePage GoToHomepage();
    }
}