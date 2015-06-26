using Moq;
using NUnit.Framework;
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
    }
}