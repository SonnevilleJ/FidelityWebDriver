using Moq;
using NUnit.Framework;

namespace Sonneville.FidelityWebDriver.Demo.Tests
{
    [TestFixture]
    public class AppTests
    {
        private string[] _cliParams;
        private App _app;
        private Mock<IFidelityDriver> _fidelityDriverMock;

        [SetUp]
        public void Setup()
        {
            _cliParams = new string[0];

            _fidelityDriverMock = new Mock<IFidelityDriver>();

            _app = new App(_fidelityDriverMock.Object);
        }

        [Test]
        public void ShouldOpenFidelityHomepage()
        {
            _app.Run(_cliParams);

            _fidelityDriverMock.Verify(driver => driver.GoToHomepage());
        }

        [Test]
        public void ShouldDisposeFidelityDriverWhenRun()
        {
            _fidelityDriverMock.Setup(driver => driver.GoToHomepage())
                .Callback(() => _fidelityDriverMock.Verify(fd => fd.Dispose(), Times.Never()));

            _app.Run(_cliParams);

            _fidelityDriverMock.Verify(driver => driver.Dispose());
        }

        [Test]
        public void ShouldObserveAutoCloseSettingWhenRun()
        {
            try
            {
                Settings.Default.AutoCloseSelenium = false;

                _app.Run(_cliParams);

                _fidelityDriverMock.Verify(driver => driver.Dispose(), Times.Never());
            }
            finally
            {
                Settings.Default.Reset();
            }
        }

        [Test]
        public void ShouldDisposeFidelityDriver()
        {
            _app.Dispose();

            _fidelityDriverMock.Verify(driver => driver.Dispose());
        }

        [Test]
        public void ShouldHandleMultipleDisposals()
        {
            _app.Dispose();
            _app.Dispose();
        }
    }
}