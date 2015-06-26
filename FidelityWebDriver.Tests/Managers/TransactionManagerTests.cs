using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Managers;

namespace Sonneville.FidelityWebDriver.Tests.Managers
{
    [TestFixture]
    public class TransactionManagerTests
    {
        private TransactionManager _manager;
        private Mock<ISiteNavigator> _siteNavigatorMock;

        [SetUp]
        public void Setup()
        {
            _siteNavigatorMock = new Mock<ISiteNavigator>();

            _manager = new TransactionManager(_siteNavigatorMock.Object);
        }

        [Test]
        public void ShouldOpenFidelityHomepage()
        {
            _manager.DownloadTransactions();

            _siteNavigatorMock.Verify(driver => driver.GoToHomepage());
        }

        [Test]
        public void ShouldDisposeFidelityDriverWhenRun()
        {
            _siteNavigatorMock.Setup(driver => driver.GoToHomepage())
                .Callback(() => _siteNavigatorMock.Verify(fd => fd.Dispose(), Times.Never()));

            _manager.DownloadTransactions();

            _siteNavigatorMock.Verify(driver => driver.Dispose());
        }

        [Test]
        public void ShouldObserveAutoCloseSettingWhenRun()
        {
            try
            {
                Settings.Default.AutoCloseSelenium = false;

                _manager.DownloadTransactions();

                _siteNavigatorMock.Verify(driver => driver.Dispose(), Times.Never());
            }
            finally
            {
                Settings.Default.Reset();
            }
        }

        [Test]
        public void ShouldDisposeFidelityDriver()
        {
            _manager.Dispose();

            _siteNavigatorMock.Verify(driver => driver.Dispose());
        }

        [Test]
        public void ShouldHandleMultipleDisposals()
        {
            _manager.Dispose();
            _manager.Dispose();
        }
    }
}