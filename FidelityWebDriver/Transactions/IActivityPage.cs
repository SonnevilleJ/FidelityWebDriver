﻿using System;
using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Transactions
{
    public interface IActivityPage : IPage
    {
        string DownloadHistory(DateTime minValue, DateTime maxValue);
    }
}