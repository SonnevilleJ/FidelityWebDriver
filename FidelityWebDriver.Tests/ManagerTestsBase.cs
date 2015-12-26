using Moq;
using NUnit.Framework;
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
            Manager = InstantiateManager(SiteNavigatorMock.Object);
        }

        protected abstract T InstantiateManager(ISiteNavigator siteNavigator);

        [Test]
        public void ShouldDisposeSiteNavigator()
        {
            Manager.Dispose();

            SiteNavigatorMock.Verify(driver => driver.Dispose());
        }

        [Test]
        public void ShouldHandleMultipleDisposals()
        {
            Manager.Dispose();
            Manager.Dispose();
        }
    }
}