using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Sonneville.FidelityWebDriver.CSV;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Tests.CSV
{
    [TestFixture]
    public class TransactionsMapperTests
    {
        private TransactionsMapper _transactionsMapper;
        private Mock<IFidelityCsvColumnMapper> _columnMapperMock;
        private Dictionary<FidelityCsvColumn, int> _columnMappings;
        private Mock<ITransactionMapper> _transactionMapperMock;
        private string _headerRow;

        [SetUp]
        public void Setup()
        {
            _headerRow = "csv headers";
            _columnMappings = new Dictionary<FidelityCsvColumn, int>();

            _columnMapperMock = new Mock<IFidelityCsvColumnMapper>();
            _columnMapperMock.Setup(mapper => mapper.GetColumnMappings(_headerRow)).Returns(_columnMappings);

            _transactionMapperMock = new Mock<ITransactionMapper>();

            _transactionsMapper = new TransactionsMapper(_columnMapperMock.Object, _transactionMapperMock.Object);
        }

        [Test]
        public void ShouldMapColumnsAndPassEachRecordToTransactionMapper()
        {
            var records = new[]
            {
                "record 1",
                "second record"
            };
            var expectedTransactions = SetupExpectedTransactions(records).ToList();
            var csvContent = SetupCsv(_headerRow, records);

            var actualTransactions = _transactionsMapper.ParseCsv(csvContent);

            CollectionAssert.AreEquivalent(expectedTransactions, actualTransactions);
        }

        private IEnumerable<IFidelityTransaction> SetupExpectedTransactions(IEnumerable<string> records)
        {
            foreach (var record in records)
            {
                var transactionMock = new Mock<IFidelityTransaction>();
                _transactionMapperMock.Setup(mapper => mapper.CreateTransaction(record, _columnMappings))
                    .Returns(transactionMock.Object);
                yield return transactionMock.Object;
            }
        }

        private static string SetupCsv(string headerRow, params string[] records)
        {
            return $"{headerRow}{Environment.NewLine}{string.Join(Environment.NewLine, records)}";
        }
    }
}