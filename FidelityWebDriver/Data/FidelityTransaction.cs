using System;

namespace Sonneville.FidelityWebDriver.Data
{
    public class FidelityTransaction
    {
        public FidelityTransaction(DateTime runDate, string account, string action, TransactionType type,
            string symbol, string securityDescription, string securityType, decimal quantity, decimal price,
            decimal commission, decimal fees, decimal accruedInterest, decimal amount, DateTime settlementDate)
        {
            RunDate = runDate;
            Account = account;
            Action = action;
            Type = type;
            Symbol = symbol;
            SecurityDescription = securityDescription;
            SecurityType = securityType;
            Quantity = quantity;
            Price = price;
            Commission = commission;
            Fees = fees;
            AccruedInterest = accruedInterest;
            Amount = amount;
            SettlementDate = settlementDate;
        }

        public DateTime RunDate { get; }

        public string Account { get; }

        public string Action { get; }

        public TransactionType Type { get; }

        public string Symbol { get; }

        public string SecurityDescription { get; }

        public string SecurityType { get; }

        public decimal Quantity { get; }

        public decimal Price { get; }

        public decimal Commission { get; }

        public decimal Fees { get; }

        public decimal AccruedInterest { get; }

        public decimal Amount { get; }

        public DateTime SettlementDate { get; }
    }
}