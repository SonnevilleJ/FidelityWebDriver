using NUnit.Framework;
using Sonneville.FidelityWebDriver.Data;
using Sonneville.FidelityWebDriver.Transactions.CSV;

namespace Sonneville.FidelityWebDriver.Tests.Transactions.Csv
{
    [TestFixture]
    public class TransactionTypeMapperTests
    {
        [Test]
        [TestCase("DIVIDEND RECEIVED", TransactionType.DividendReceipt)]
        [TestCase("REINVESTMENT", TransactionType.DividendReinvestment)]
        [TestCase("YOU SOLD             EXCHANGE", TransactionType.Sell)]
        [TestCase("YOU BOUGHT           PROSPECTUS UNDER    SEPARATE COVER", TransactionType.Buy)]
        [TestCase("SHORT-TERM CAP GAIN", TransactionType.DividendReceipt)]
        [TestCase("LONG-TERM CAP GAIN", TransactionType.DividendReceipt)]
        [TestCase("Electronic Funds Transfer Received", TransactionType.Deposit)]
        [TestCase("TRANSFERRED FROM     TO BROKERAGE OPTION", TransactionType.Deposit)]
        [TestCase("PARTIC CONTR CURRENT PARTICIPANT CUR YR", TransactionType.Deposit)]
        [TestCase("abcdefghijklmnopqrstuvwxyz", TransactionType.Unknown)]
        public void ShouldMapValues(string value, TransactionType expectedType)
        {
            var actualType = new TransactionTypeMapper().Map(value);

            Assert.AreEqual(expectedType, actualType);
        }
    }
}