using Moq;
using Ninject;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Demo.Ninject;

namespace Sonneville.FidelityWebDriver.Demo.Tests.Ninject
{
    [TestFixture]
    public class KernelBuilderTests
    {
        private Mock<IWebDriver> _webDriverMock;
        private IKernel _kernel;

        [SetUp]
        public void Setup()
        {
            // mock out web driver because these tests focus on Ninject bindings, not Selenium
            _webDriverMock = new Mock<IWebDriver>();

            _kernel = new KernelBuilder().Build();
            _kernel.Rebind<IWebDriver>().ToConstant(_webDriverMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _kernel?.Dispose();
        }

        [Test]
        public void ShouldGetApp()
        {
            using (var app = _kernel.Get<IApp>())
            {
                Assert.IsNotNull(app);
            }
        }

        [Test]
        public void ShouldGetSameConfigEachTime()
        {
            var config1 = _kernel.Get<FidelityConfiguration>();
            var config2 = _kernel.Get<FidelityConfiguration>();

            Assert.AreSame(config1, config2);
        }

        [Test]
        public void ShouldNotDisposeMoreThanOnce()
        {
            _kernel.Get<IApp>().Dispose();

            _webDriverMock.Verify(webDriver => webDriver.Dispose(), Times.Once());
        }
    }
}