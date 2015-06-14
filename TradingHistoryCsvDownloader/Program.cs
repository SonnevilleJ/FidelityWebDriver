using Ninject;
using Sonneville.TradingHistoryCsvDownloader.Ninject;

namespace Sonneville.TradingHistoryCsvDownloader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new KernelBuilder().Build().Get<IApp>().Run(args);
        }
    }
}
