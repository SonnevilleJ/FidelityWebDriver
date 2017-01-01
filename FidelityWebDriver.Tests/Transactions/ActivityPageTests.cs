﻿using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Navigation;
using Sonneville.FidelityWebDriver.Tests.Navigation;
using Sonneville.FidelityWebDriver.Transactions;

namespace Sonneville.FidelityWebDriver.Tests.Transactions
{
    [TestFixture]
    public class ActivityPageTests : PageFactoryTests<IActivityPage>
    {
        private DateTime _expectedStartDate;
        private DateTime _expectedEndDate;
        private List<IFidelityTransaction> _expectedTransactions;

        private Mock<IWebElement> _historyRootDivMock;
        private Mock<IWebElement> _historyExpanderLinkMock;
        private Mock<IWebElement> _historyRangeDropdownMock;
        private Mock<IWebElement> _customHistoryRangeOptionMock;
        private Mock<IWebElement> _setTimePeriodButtonMock;
        private Mock<IWebElement> _dateRangeDivMock;
        private Mock<IWebElement> _fromDateInputMock;
        private Mock<IWebElement> _toDateInputMock;
        private Mock<IWebElement> _progressBarDivMock;

        private Mock<IWebDriver> _webDriverMock;
        private Mock<IPageFactory> _pageFactoryMock;
        private Mock<IHistoryTransactionParser> _historyTransactionParserMock;

        private ActivityPage _activityPage;

        [SetUp]
        public void Setup()
        {
            SetupPageFactory();

            _expectedStartDate = DateTime.Today.AddDays(-30);
            _expectedEndDate = DateTime.Today;

            _webDriverMock = new Mock<IWebDriver>();

            _historyRootDivMock = new Mock<IWebElement>();
            _historyRootDivMock.Setup(div => div.GetAttribute("class")).Returns("activity--expander activity--expander-history expander last collapsed");

            _webDriverMock.Setup(driver => driver.FindElement(By.ClassName("activity--expander-history")))
                .Returns(_historyRootDivMock.Object);

            _customHistoryRangeOptionMock = new Mock<IWebElement>();
            _customHistoryRangeOptionMock.Setup(option => option.Click())
                .Callback(() => _customHistoryRangeOptionMock.Setup(option => option.Selected).Returns(true));

            _historyRangeDropdownMock = new Mock<IWebElement>();
            _historyRangeDropdownMock.Setup(dropdown => dropdown.FindElement(By.CssSelector("option[value='custom']")))
                .Returns(_customHistoryRangeOptionMock.Object);
            _historyRootDivMock.Setup(div => div.FindElement(By.Id("activity--history-range-dropdown")))
                .Returns(_historyRangeDropdownMock.Object);

            _setTimePeriodButtonMock = new Mock<IWebElement>();

            _historyExpanderLinkMock = new Mock<IWebElement>();
            _historyRootDivMock.Setup(div => div.FindElement(By.Id("historyExpander")))
                .Returns(_historyExpanderLinkMock.Object);

            _fromDateInputMock = new Mock<IWebElement>();

            _toDateInputMock = new Mock<IWebElement>();

            _dateRangeDivMock = new Mock<IWebElement>();
            _dateRangeDivMock.Setup(div => div.FindElement(By.ClassName("activity--history-custom-date-from-field")))
                .Returns(_fromDateInputMock.Object);
            _dateRangeDivMock.Setup(div => div.FindElement(By.ClassName("activity--history-custom-date-to-field")))
                .Returns(_toDateInputMock.Object);
            _dateRangeDivMock.Setup(
                    div => div.FindElement(By.ClassName("activity--history-custom-date-display-button")))
                .Returns(_setTimePeriodButtonMock.Object);
            _historyRootDivMock.Setup(
                    div => div.FindElement(By.ClassName("activity--history-custom-date-container")))
                .Returns(_dateRangeDivMock.Object);

            _progressBarDivMock = new Mock<IWebElement>();

            _webDriverMock.Setup(webDriver => webDriver.FindElement(By.ClassName("progress-bar")))
                .Returns(_progressBarDivMock.Object);

            _pageFactoryMock = new Mock<IPageFactory>();

            _expectedTransactions = new List<IFidelityTransaction>
            {
                new FidelityTransaction()
            };

            _historyTransactionParserMock = new Mock<IHistoryTransactionParser>();
            _historyTransactionParserMock.Setup(parser => parser.ParseFidelityTransactions(_historyRootDivMock.Object))
                .Returns(_expectedTransactions);

            _activityPage = new ActivityPage(_webDriverMock.Object, _pageFactoryMock.Object,
                _historyTransactionParserMock.Object);
        }

        [Test]
        public void GetHistoryShouldWaitAfterExpandingHistory()
        {
            SetupVisibleProgressBar();
            _historyExpanderLinkMock.Setup(link => link.Click())
                .Callback(AssertInvisibleProgressBar);

            _activityPage.GetTransactions(_expectedStartDate, _expectedEndDate);

            _historyExpanderLinkMock.Verify(link => link.Click());
        }

        [Test]
        public void ShouldNotExpandHistoryIfAlreadyExpanded()
        {
            _historyRootDivMock.Setup(div => div.GetAttribute("class")).Returns("activity--expander activity--expander-history expander last expanded");

            _activityPage.GetTransactions(_expectedStartDate, _expectedEndDate);

            _historyExpanderLinkMock.Verify(link => link.Click(), Times.Never);
        }

        [Test]
        public void GetHistoryShouldSetTimePeriod()
        {
            _historyExpanderLinkMock.Setup(link => link.Click())
                .Callback(SetupVisibleProgressBar);
            _setTimePeriodButtonMock.Setup(button => button.Click())
                .Callback(() =>
                {
                    _fromDateInputMock.Verify(input => input.SendKeys(_expectedStartDate.ToString("MM/dd/yyyy")));
                    _toDateInputMock.Verify(input => input.SendKeys(_expectedEndDate.ToString("MM/dd/yyyy")));
                    Assert.IsTrue(_customHistoryRangeOptionMock.Object.Selected);
                    AssertInvisibleProgressBar();
                });

            _activityPage.GetTransactions(_expectedStartDate, _expectedEndDate);

            _setTimePeriodButtonMock.Verify(button => button.Click());
        }

        [Test]
        public void GetHistoryShouldWaitAfterSettingTimePeriod()
        {
            _setTimePeriodButtonMock.Setup(button => button.Click())
                .Callback(SetupVisibleProgressBar);
            _historyTransactionParserMock.Setup(parser => parser.ParseFidelityTransactions(_historyRootDivMock.Object))
                .Callback(AssertInvisibleProgressBar);

            _activityPage.GetTransactions(_expectedStartDate, _expectedEndDate);

            _historyTransactionParserMock.Verify(
                parser => parser.ParseFidelityTransactions(_historyRootDivMock.Object));
        }

        [Test]
        public void GetHistoryShouldReturnTransactions()
        {
            var actualTransactions = _activityPage.GetTransactions(_expectedStartDate, _expectedEndDate);

            AssertInvisibleProgressBar();
            CollectionAssert.AreEquivalent(_expectedTransactions, actualTransactions);
        }

        [Test]
        [TestCase("11/12/2013", "11/11/2013")]
        [TestCase("11/12/2012", "11/11/2013")]
        [TestCase("11/11/9000", "11/12/9000")]
        public void ShouldValidateDates(string startDateString, string endDateString)
        {
            _expectedStartDate = DateTime.Parse(startDateString);
            _expectedEndDate = DateTime.Parse(endDateString);

            Assert.Throws<ArgumentException>(() => _activityPage.GetTransactions(_expectedStartDate, _expectedEndDate));
        }

        private void SetupVisibleProgressBar()
        {
            _progressBarDivMock.Setup(div => div.Displayed)
                .Returns(() =>
                {
                    try
                    {
                        return true;
                    }
                    finally
                    {
                        _progressBarDivMock.Setup(div => div.Displayed).Returns(false);
                    }
                });
        }

        private void AssertInvisibleProgressBar()
        {
            Assert.IsFalse(_progressBarDivMock.Object.Displayed, "Progress bar div is obstructing element!");
        }
    }
}