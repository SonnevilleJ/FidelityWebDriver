using System.Collections.Generic;

namespace Sonneville.FidelityWebDriver.Data
{
    public interface IAccountDetails
    {
        AccountType AccountType { get; }

        string Name { get; }

        string AccountNumber { get; }

        decimal PendingActivity { get; }

        decimal TotalGainDollar { get; }

        decimal TotalGainPercent { get; }

        IList<IPosition> Positions { get; }
    }
}