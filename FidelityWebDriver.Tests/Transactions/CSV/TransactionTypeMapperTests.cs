using NUnit.Framework;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Transactions.CSV;

namespace Sonneville.FidelityWebDriver.Tests.Transactions.Csv
{
    [TestFixture]
    public class TransactionTypeMapperTests
    {
        private TransactionTypeMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new TransactionTypeMapper();
        }

        [Test]
        public void ShouldMapDividendReceived()
        {
            var transactionType = _mapper.Map("DIVIDEND RECEIVED");

            Assert.AreEqual(TransactionType.DividendReceipt, transactionType);
        }

        [Test]
        public void ShouldMapDividendReinvestment()
        {
            var transactionType = _mapper.Map("REINVESTMENT");

            Assert.AreEqual(TransactionType.DividendReinvestment, transactionType);
        }

        [Test]
        public void ShouldMapSell()
        {
            var transactionType = _mapper.Map("YOU SOLD             EXCHANGE");

            Assert.AreEqual(TransactionType.Sell, transactionType);
        }

        [Test]
        public void ShouldMapBuy()
        {
            var transactionType = _mapper.Map("YOU BOUGHT           PROSPECTUS UNDER    SEPARATE COVER");

            Assert.AreEqual(TransactionType.Buy, transactionType);
        }

        [Test]
        public void ShouldMapShortTermCapitalGain()
        {
            var transactionType = _mapper.Map("SHORT-TERM CAP GAIN");

            Assert.AreEqual(TransactionType.DividendReceipt, transactionType);
        }

        [Test]
        public void ShouldMapLongTermCapitalGain()
        {
            var transactionType = _mapper.Map("LONG-TERM CAP GAIN");

            Assert.AreEqual(TransactionType.DividendReceipt, transactionType);
        }

        [Test]
        public void ShouldMapIndividualDeposit()
        {
            var transactionType = _mapper.Map("Electronic Funds Transfer Received");

            Assert.AreEqual(TransactionType.Deposit, transactionType);
        }

        [Test]
        public void ShouldMapBrokeragelinkDeposit()
        {
            var transactionType = _mapper.Map("TRANSFERRED FROM     TO BROKERAGE OPTION");

            Assert.AreEqual(TransactionType.Deposit, transactionType);
        }

        [Test]
        public void ShouldMapHsaDeposit()
        {
            var transactionType = _mapper.Map("PARTIC CONTR CURRENT PARTICIPANT CUR YR");

            Assert.AreEqual(TransactionType.Deposit, transactionType);
        }
    }
}