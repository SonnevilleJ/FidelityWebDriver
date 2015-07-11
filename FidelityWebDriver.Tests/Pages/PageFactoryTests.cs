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
        protected Mock<IWebDriver> WebDriverMock;

        [SetUp]
        public void SetupPageFactory()
        {
            WebDriverMock = new Mock<IWebDriver>();

            _factory = new PageFactory(WebDriverMock.Object);
        }

        [Test]
        public void ShouldReturnSamePageForEachRequest()
        {
            var homePage1 = _factory.GetPage<T>();
            var homePage2 = _factory.GetPage<T>();

            Assert.AreSame(homePage1, homePage2);
        }
    }
}