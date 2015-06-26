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
        public void ShouldCompleteDemoAppWithoutErrors()
        {
            if (string.IsNullOrEmpty(Settings.Default.Username) || string.IsNullOrEmpty(Settings.Default.Password))
            {
                Assert.Inconclusive("Username or password not set; unable to log into Fidelity without credentials!");
            }

            var app = _kernelBuilder.Build().Get<IApp>();
            try
            {
                app.Run(new string[0]);
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

        /// <summary>
        /// Use this to set the Fidelity credentials for your user account
        /// </summary>
        [Test]
        [Ignore]
        public void SetCredentials()
        {
            Settings.Default.Username = "";
            Settings.Default.Password = "";
            Settings.Default.Save();
        }
    }
}