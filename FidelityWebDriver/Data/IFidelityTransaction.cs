using System;

namespace Sonneville.FidelityWebDriver.Data
{
    public interface IFidelityTransaction
    {
        DateTime? RunDate { get; }

        string AccountName { get; }

        string AccountNumber { get; }

        string Action { get; }

        TransactionType Type { get; }

        string Symbol { get; }

        string SecurityDescription { get; }

        string SecurityType { get; }

        decimal? Quantity { get; }

        decimal? Price { get; }

        decimal? Commission { get; }

        decimal? Fees { get; }

        decimal? AccruedInterest { get; }

        decimal? Amount { get; }

        DateTime? SettlementDate { get; }
    }
}