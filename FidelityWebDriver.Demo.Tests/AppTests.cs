using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Managers;

namespace Sonneville.FidelityWebDriver.Demo.Tests
{
    [TestFixture]
    public class AppTests
    {
        private string[] _args;
        private App _app;
        private Mock<ITransactionManager> _transactionManagerMock;
        private FidelityConfiguration _fidelityConfiguration;

        [SetUp]
        public void Setup()
        {
            _args = new string[0];

            _transactionManagerMock = new Mock<ITransactionManager>();

            _fidelityConfiguration = new FidelityConfiguration();
            _fidelityConfiguration.Initialize();

            _app = new App(_transactionManagerMock.Object, _fidelityConfiguration);
        }

        [TearDown]
        public void Teardown()
        {
            _fidelityConfiguration.Username = null;
            _fidelityConfiguration.Password = null;
            _fidelityConfiguration.Write();
        }

        [Test]
        public void ShouldDelegateToTransactionManager()
        {
            _app.Run(_args);

            _transactionManagerMock.Verify(manager => manager.DownloadTransactions());
        }

        [Test]
        public void ShouldSetConfigFromCliArgsWithoutPersisting()
        {
            const string cliUserName = "Batman";
            const string cliPassword = "I am vengeance. I am the night. I am Batman.";
            _args = new[] {"-u", cliUserName, "-p", cliPassword};

            _app.Run(_args);

            Assert.AreEqual(cliUserName, _fidelityConfiguration.Username);
            Assert.AreEqual(cliPassword, _fidelityConfiguration.Password);
            _fidelityConfiguration.Read();
            Assert.IsNull(_fidelityConfiguration.Username);
            Assert.IsNull(_fidelityConfiguration.Password);
        }

        [Test]
        public void ShouldPersistConfigFromCliArgs()
        {
            const string cliUserName = "Batman";
            const string cliPassword = "I am vengeance. I am the night. I am Batman.";
            _args = new[] {"-u", cliUserName, "-p", cliPassword, "-save"};

            _app.Run(_args);

            var fidelityConfiguration = new FidelityConfiguration();
            fidelityConfiguration.Initialize();
            Assert.AreEqual(cliUserName, fidelityConfiguration.Username);
            Assert.AreEqual(cliPassword, fidelityConfiguration.Password);
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
    }
}