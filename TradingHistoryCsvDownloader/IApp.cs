using System;

namespace Sonneville.TradingHistoryCsvDownloader
{
    public interface IApp : IDisposable
    {
        void Run(string[] args);
    }
}