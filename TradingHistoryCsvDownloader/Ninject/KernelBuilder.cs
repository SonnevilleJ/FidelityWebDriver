using Ninject;

namespace Sonneville.TradingHistoryCsvDownloader.Ninject
{
    public class KernelBuilder
    {
        public IKernel Build()
        {
            return new StandardKernel(new AppModule(), new SeleniumModule());
        } 
    }
}