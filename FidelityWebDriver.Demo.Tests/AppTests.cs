using Moq;
using Ninject;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Demo.Ninject;
using Sonneville.FidelityWebDriver.Managers;

namespace Sonneville.FidelityWebDriver.Demo.Tests
{
    [TestFixture]
    public class AppTests
    {
        private string[] _args;
        private App _app;
        private Mock<ITransactionManager> _transactionManagerMock;

        [SetUp]
        public void Setup()
        {
            _args = new string[0];

            _transactionManagerMock = new Mock<ITransactionManager>();

            _app = new App(_transactionManagerMock.Object);
        }

        [Test]
        public void ShouldDelegateToTransactionManager()
        {
            _app.Run(_args);

            _transactionManagerMock.Verify(manager => manager.DownloadTransactions());
        }

        [Test]
        public void ShouldCascadeDisposeToManagers()
        {
            _app.Dispose();

            _transactionManagerMock.Verify(manager => manager.Dispose());
        }

        [Test]
        public void ShouldHandleMultipleDisposals()
        {
            _app.Dispose();
            _app.Dispose();
        }

        [Test]
        public void ShouldCompleteDemoAppWithoutErrors()
        {
            if (string.IsNullOrEmpty(Settings.Default.Username) || string.IsNullOrEmpty(Settings.Default.Password))
            {
                Assert.Inconclusive("Username or password not set; unable to log into Fidelity without credentials!");
            }

            var app = new KernelBuilder().Build().Get<IApp>();
            try
            {
                app.Run(new string[0]);
            }
            finally
            {
                app.Dispose();
            }
        }

        /// <summary>
        /// Use this to set the Fidelity credentials for your user account
        /// </summary>
        [Test]
        public void SetCredentials()
        {
            Settings.Default.Username = "";
            Settings.Default.Password = "";
            
            if (string.IsNullOrEmpty(Settings.Default.Username) || string.IsNullOrEmpty(Settings.Default.Password))
            {
                Assert.Inconclusive("Username or password not set; not going to persist empty credentials!");
            }
            else
            {
                Settings.Default.Save();
                Assert.Pass("Credentials saved successfully!");
            }
        }
    }
}