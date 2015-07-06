﻿using NUnit.Framework;
using Sonneville.FidelityWebDriver.Configuration;
using Westwind.Utilities.Configuration;

namespace Sonneville.FidelityWebDriver.Tests.Configuration
{
    [TestFixture]
    public class IsolatedStorageConfigurationProviderTests
    {
        private IsolatedStorageConfigurationProvider<AppConfigurationInheritor> _provider;
        private AppConfigurationInheritor _config;
        private string _value1;

        public class AppConfigurationInheritor : AppConfiguration
        {
            public string Value1 { get; set; }
        }

        [SetUp]
        public void Setup()
        {
            _value1 = "SomeConfigValue";
            _config = new AppConfigurationInheritor();
            _config.Initialize();
            _config.Value1 = _value1;

            _provider = new IsolatedStorageConfigurationProvider<AppConfigurationInheritor>();
        }

        [TearDown]
        public void Teardown()
        {
            _config = new AppConfigurationInheritor();
            _config.Initialize();
            _config.Value1 = null;
            _config.Write();
        }

        [Test]
        public void RoundtripTest()
        {
            var write = _provider.Write(_config);
            Assert.IsTrue(write);

            var config2 = new AppConfigurationInheritor();
            config2.Initialize(_provider);
            Assert.AreEqual(_config.Value1, config2.Value1);
        }

        [Test]
        public void EncryptionTest()
        {
            var encryptingProvider = new IsolatedStorageConfigurationProvider<AppConfigurationInheritor>
            {
                PropertiesToEncrypt = "Value1",
                EncryptionKey = "asdf"
            };

            var write = encryptingProvider.Write(_config);
            Assert.IsTrue(write);

            var config2 = new AppConfigurationInheritor();
            config2.Initialize(_provider);
            Assert.AreNotEqual(_value1, config2.Value1);
        }
    }
}