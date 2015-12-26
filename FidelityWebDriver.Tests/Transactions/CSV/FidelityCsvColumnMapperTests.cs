using System.Linq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.Transactions.CSV;

namespace Sonneville.FidelityWebDriver.Tests.Transactions.Csv
{
    [TestFixture]
    public class FidelityCsvColumnMapperTests
    {
        [Test]
        public void ShouldMapAndTrimUnknown()
        {
            var actual = new FidelityCsvColumnMapper().GetHeader(" gibberish ");

            Assert.AreEqual(FidelityCsvColumn.Unknown, actual);
        }

        [Test]
        public void ShouldMapAndTrimAccount()
        {
            var actual = new FidelityCsvColumnMapper().GetHeader(" Account ");

            Assert.AreEqual(FidelityCsvColumn.Account, actual);
        }

        [Test]
        public void ShouldMapAndTrimAction()
        {
            var actual = new FidelityCsvColumnMapper().GetHeader(" Action ");

            Assert.AreEqual(FidelityCsvColumn.Action, actual);
        }

        [Test]
        public void ShouldMapAndTrimSymbol()
        {
            var actual = new FidelityCsvColumnMapper().GetHeader(" Symbol ");

            Assert.AreEqual(FidelityCsvColumn.Symbol, actual);
        }

        [Test]
        public void ShouldMapAndTrimSecurityDescription()
        {
            var actual = new FidelityCsvColumnMapper().GetHeader(" Security Description ");

            Assert.AreEqual(FidelityCsvColumn.SecurityDescription, actual);
        }

        [Test]
        public void ShouldMapAndTrimSecurityType()
        {
            var actual = new FidelityCsvColumnMapper().GetHeader(" Security Type ");

            Assert.AreEqual(FidelityCsvColumn.SecurityType, actual);
        }

        [Test]
        public void ShouldMapAndTrimQuantity()
        {
            var actual = new FidelityCsvColumnMapper().GetHeader(" Quantity ");

            Assert.AreEqual(FidelityCsvColumn.Quantity, actual);
        }

        [Test]
        public void ShouldMapAndTrimPrice()
        {
            var actual = new FidelityCsvColumnMapper().GetHeader(" Price ($) ");

            Assert.AreEqual(FidelityCsvColumn.Price, actual);
        }

        [Test]
        public void ShouldMapAndTrimCommission()
        {
            var actual = new FidelityCsvColumnMapper().GetHeader(" Commission ($) ");

            Assert.AreEqual(FidelityCsvColumn.Commission, actual);
        }

        [Test]
        public void ShouldMapAndTrimFees()
        {
            var actual = new FidelityCsvColumnMapper().GetHeader(" Fees ($) ");

            Assert.AreEqual(FidelityCsvColumn.Fees, actual);
        }

        [Test]
        public void ShouldMapAndTrimAccruedInterest()
        {
            var actual = new FidelityCsvColumnMapper().GetHeader(" Accrued Interest ($) ");

            Assert.AreEqual(FidelityCsvColumn.AccruedInterest, actual);
        }

        [Test]
        public void ShouldMapAndTrimAmount()
        {
            var actual = new FidelityCsvColumnMapper().GetHeader(" Amount ($) ");

            Assert.AreEqual(FidelityCsvColumn.Amount, actual);
        }

        [Test]
        public void ShouldMapAndTrimSettlementDate()
        {
            var actual = new FidelityCsvColumnMapper().GetHeader(" Settlement Date ");

            Assert.AreEqual(FidelityCsvColumn.SettlementDate, actual);
        }

        [Test]
        public void ShouldMapMultipleColumns()
        {
            var headers = new FidelityCsvColumnMapper().GetColumnMappings($"{" Account "}, {" Settlement Date "}, {" Amount ($) "}");

            Assert.AreEqual(0, headers[FidelityCsvColumn.Account]);
            Assert.AreEqual(1, headers[FidelityCsvColumn.SettlementDate]);
            Assert.AreEqual(2, headers[FidelityCsvColumn.Amount]);
            Assert.AreEqual(3, headers.Count());
        }
    }
}