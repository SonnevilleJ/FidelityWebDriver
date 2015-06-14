using Sonneville.FidelityWebDriver;

namespace Sonneville.TradingHistoryCsvDownloader
{
    public class App : IApp
    {
        private readonly IFidelityDriver _fidelityDriver;

        public App(IFidelityDriver fidelityDriver)
        {
            _fidelityDriver = fidelityDriver;
        }

        public void Run(string[] args)
        {
            _fidelityDriver.GoToHomepage();

            if (Settings.Default.AutoCloseSelenium)
            {
                _fidelityDriver.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var fidelityDriver = _fidelityDriver;
                if (fidelityDriver != null) fidelityDriver.Dispose();
            }
        }
    }
}