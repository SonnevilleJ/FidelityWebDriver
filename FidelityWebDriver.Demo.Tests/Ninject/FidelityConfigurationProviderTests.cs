﻿using System.IO;
using Nini.Config;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Demo.Ninject;

namespace Sonneville.FidelityWebDriver.Demo.Tests.Ninject
{
    [TestFixture]
    public class FidelityConfigurationProviderTests
    {
        private FidelityConfigurationProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new FidelityConfigurationProvider();
        }

        [TearDown]
        public static void Teardown()
        {
            DeletePersistedConfig();
        }

        [Test]
        public void ShouldSetType()
        {
            Assert.AreEqual(typeof(FidelityConfiguration), _provider.Type);
        }

        [Test]
        public void ShouldReturnBlankConfig()
        {
            var createdConfig = _provider.Create(null) as FidelityConfiguration;

            Assert.IsNotNull(createdConfig);
            Assert.IsEmpty(createdConfig.Username);
            Assert.IsEmpty(createdConfig.Password);
        }

        [Test]
        public void ShouldReturnPersistedConfig()
        {
            var fidelityConfiguration = SetupConfiguration("user name", "pass word");
            PersistConfiguration(fidelityConfiguration);

            var createdConfig = _provider.Create(null) as FidelityConfiguration;

            Assert.IsNotNull(createdConfig);
            Assert.AreEqual(fidelityConfiguration.Username, createdConfig.Username);
            Assert.AreEqual(fidelityConfiguration.Password, createdConfig.Password);
        }

        public static FidelityConfiguration ReadConfiguration()
        {
            if (File.Exists("./demo.ini"))
            {
                var iniConfigSource = new IniConfigSource("./demo.ini");
                var config = iniConfigSource.Configs["Fidelity"];
                return new FidelityConfiguration
                {
                    Username = config.Get("Username"),
                    Password = config.Get("Password"),
                };
            }
            return new FidelityConfiguration();
        }

        public static FidelityConfiguration SetupConfiguration(string username, string password)
        {
            var configuration = new FidelityConfiguration
            {
                Username = username,
                Password = password,
            };

            return configuration;
        }

        public static void DeletePersistedConfig()
        {
            if (File.Exists("./demo.ini"))
            {
                File.Delete("./demo.ini");
            }
        }

        public static void PersistConfiguration(FidelityConfiguration configuration)
        {
            File.Create("./demo.ini").Dispose();
            var iniConfigSource = new IniConfigSource("./demo.ini");
            iniConfigSource.AddConfig("Fidelity");
            var config = iniConfigSource.Configs["Fidelity"];
            config.Set("Username", configuration.Username);
            config.Set("Password", configuration.Password);
            iniConfigSource.Save();
        }
    }
}