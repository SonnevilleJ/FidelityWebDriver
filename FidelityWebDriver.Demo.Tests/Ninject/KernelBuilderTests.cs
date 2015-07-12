using Ninject;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Sonneville.FidelityWebDriver.Demo.Ninject;

namespace Sonneville.FidelityWebDriver.Demo.Tests.Ninject
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
            using (var app = _kernelBuilder.Build().Get<IApp>())
            {
                Assert.IsNotNull(app);
            }
        }

        [Test]
        public void ShouldGetChromeDriver()
        {
            using (var webDriver = _kernelBuilder.Build().Get<IWebDriver>())
            {
                Assert.IsInstanceOf<ChromeDriver>(webDriver);
            }
        }
    }
}