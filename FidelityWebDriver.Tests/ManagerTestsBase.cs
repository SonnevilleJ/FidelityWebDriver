using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Configuration;
using Sonneville.FidelityWebDriver.Navigation;

namespace Sonneville.FidelityWebDriver.Tests
{
    [TestFixture]
    public abstract class ManagerTestsBase<T> where T : IManager
    {
        protected T Manager;
        protected Mock<ISiteNavigator> SiteNavigatorMock;

        [SetUp]
        public void SetupTestsBase()
        {
            SiteNavigatorMock = new Mock<ISiteNavigator>();
            Manager = InstantiateManager(SiteNavigatorMock.Object, new FidelityConfiguration());
        }

        protected abstract T InstantiateManager(ISiteNavigator siteNavigator, FidelityConfiguration fidelityConfiguration);

        [Test]
        public void EachManagerShouldDisposeSiteNavigator()
        {
            Manager.Dispose();

            SiteNavigatorMock.Verify(driver => driver.Dispose());
        }

        [Test]
        public void EachManagerShouldHandleMultipleDisposals()
        {
            Manager.Dispose();
            Manager.Dispose();
        }
    }
}