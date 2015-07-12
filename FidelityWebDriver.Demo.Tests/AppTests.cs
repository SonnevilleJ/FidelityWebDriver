using System;
using System.IO;
using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Managers;

namespace Sonneville.FidelityWebDriver.Demo.Tests
{
    [TestFixture]
    public class AppTests : IDisposable
    {
        private App _app;
        private Mock<ITransactionManager> _transactionManagerMock;
        private FidelityConfiguration _fidelityConfiguration;
        private string _cliUserName;
        private string _cliPassword;

        [SetUp]
        public void Setup()
        {
            _cliUserName = "Batman";
            _cliPassword = "I am vengeance. I am the night. I am Batman.";
            
            _transactionManagerMock = new Mock<ITransactionManager>();

            _fidelityConfiguration = new FidelityConfiguration();
            _fidelityConfiguration.Initialize();

            _app = new App(_transactionManagerMock.Object, _fidelityConfiguration);
        }

        [TearDown]
        public void Teardown()
        {
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true});

            _fidelityConfiguration.Username = null;
            _fidelityConfiguration.Password = null;
            _fidelityConfiguration.Write();
        }

        [Test]
        public void ShouldDelegateToTransactionManager()
        {
            _app.Run(new string[0]);

            _transactionManagerMock.Verify(manager => manager.DownloadTransactions());
        }

        [Test]
        public void ShouldSetConfigFromCliArgsWithoutPersisting()
        {
            var args = new[] {"-u", _cliUserName, "-p", _cliPassword};

            _app.Run(args);

            Assert.AreEqual(_cliUserName, _fidelityConfiguration.Username);
            Assert.AreEqual(_cliPassword, _fidelityConfiguration.Password);
            AssertUnchangedConfig();
        }

        [Test]
        public void ShouldSetConfigFromCliArgsAndPersist()
        {
            var args = new[] {"-u", _cliUserName, "-p", _cliPassword, "-s"};

            _app.Run(args);

            var fidelityConfiguration = new FidelityConfiguration();
            fidelityConfiguration.Initialize();
            Assert.AreEqual(_cliUserName, fidelityConfiguration.Username);
            Assert.AreEqual(_cliPassword, fidelityConfiguration.Password);
        }

        [Test]
        public void ShouldDisplayHelpFromCliArgsAndNotPersist()
        {
            using (var memoryStream = new MemoryStream())
            {
                RedirectConsoleOutput(memoryStream);

                _app.Run(new[] {"-u", _cliUserName, "-p", _cliPassword, "-s", "-h"});

                var consoleOutput = ReadConsoleOutput(memoryStream);
                Assert.IsTrue(consoleOutput.Contains("-h"),
                    string.Format("Actual console output follows:{0}{1}", Environment.NewLine, consoleOutput));
                AssertUnchangedConfig();
            }
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

        private void AssertUnchangedConfig()
        {
            _fidelityConfiguration.Read();
            Assert.IsNull(_fidelityConfiguration.Username);
            Assert.IsNull(_fidelityConfiguration.Password);
        }

        private static void RedirectConsoleOutput(Stream memoryStream)
        {
            var streamWriter = new StreamWriter(memoryStream) {AutoFlush = true};
            Console.SetOut(streamWriter);
        }

        private static string ReadConsoleOutput(Stream memoryStream)
        {
            memoryStream.Position = 0;
            return new StreamReader(memoryStream).ReadToEnd();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var app = _app;
                if (app != null)
                {
                    app.Dispose();
                    _app = null;
                }
            }
        }
    }
}