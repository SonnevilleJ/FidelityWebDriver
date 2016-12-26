using System;

namespace Sonneville.FidelityWebDriver.Data
{
    public class FidelityTransaction : IFidelityTransaction
    {
        public DateTime? RunDate { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string Action { get; set; }

        public TransactionType Type { get; set; }

        public string Symbol { get; set; }

        public string SecurityDescription { get; set; }

        public string SecurityType { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? Price { get; set; }

        public decimal? Commission { get; set; }

        public decimal? Fees { get; set; }

        public decimal? AccruedInterest { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? SettlementDate { get; set; }
    }
}