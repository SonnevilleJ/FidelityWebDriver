using System;

namespace Sonneville.FidelityWebDriver.Data
{
    public class Transaction
    {
        DateTime SettlementDate { get; }
        decimal Amount { get; }
        string Description { get; }
        string Ticker { get; }
        decimal Shares { get; }
        decimal PerSharePrice { get; }
        decimal Commission { get; }
    }
}