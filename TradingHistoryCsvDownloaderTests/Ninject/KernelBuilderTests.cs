using Ninject;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Sonneville.TradingHistoryCsvDownloader;
using Sonneville.TradingHistoryCsvDownloader.Ninject;

namespace Sonneville.TradingHistoryCsvDownloaderTests.Ninject
{
    [TestFixture]
    public class KernelBuilderTests
    {
        private KernelBuilder _kernelBuilder;

        [SetUp]
        public void Setup()
        {
            _kernelBuilder = new KernelBuilder();
        }

        [Test]
        public void ShouldGetApp()
        {
            var app = _kernelBuilder.Build().Get<IApp>();
            try
            {
                Assert.IsNotNull(app);
            }
            finally
            {
                app.Dispose();
            }
        }

        [Test]
        public void ShouldGetChromeDriver()
        {
            using (var webDriver = _kernelBuilder.Build().Get<IWebDriver>())
            {
                try
                {
                    Assert.IsInstanceOf<ChromeDriver>(webDriver);
                }
                finally
                {
                    webDriver.Close();
                }
            }
        }
    }
}