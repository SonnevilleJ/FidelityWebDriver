using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Pages;

namespace Sonneville.FidelityWebDriver.Tests.Pages
{
    [TestFixture]
    public abstract class PageFactoryTests<T> where T : IPage
    {
        private PageFactory _factory;
        private Mock<IWebDriver> _driverMock;

        [SetUp]
        public void SetupPageFactory()
        {
            _driverMock = new Mock<IWebDriver>(MockBehavior.Strict);

            _factory = new PageFactory(_driverMock.Object);
        }

        [Test]
        public void ShouldReturnSamePageForEachRequest()
        {
            var page1 = _factory.GetPage<T>();
            var page2 = _factory.GetPage<T>();

            Assert.AreSame(page1, page2);
        }
    }
}