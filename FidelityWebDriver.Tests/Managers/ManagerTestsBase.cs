using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Managers;

namespace Sonneville.FidelityWebDriver.Tests.Managers
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
            Manager = InstantiateManager();
        }

        protected abstract T InstantiateManager();

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