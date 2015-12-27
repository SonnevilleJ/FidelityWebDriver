using System.Collections.Generic;

namespace Sonneville.FidelityWebDriver.Data
{
    public class AccountDetails : IAccountDetails
    {
        public AccountDetails()
        {
        }

        public AccountDetails(AccountType accountType, string name, string accountNumber, decimal pendingActivity,
            decimal totalGainDollar, decimal totalGainPercent, IList<IPosition> positions)
        {
            AccountType = accountType;
            Name = name;
            AccountNumber = accountNumber;
            PendingActivity = pendingActivity;
            TotalGainDollar = totalGainDollar;
            TotalGainPercent = totalGainPercent;
            Positions = positions;
        }

        public AccountType AccountType { get; set; }

        public string Name { get; set; }

        public string AccountNumber { get; set; }

        public decimal PendingActivity { get; set; }

        public decimal TotalGainDollar { get; set; }

        public decimal TotalGainPercent { get; set; }

        public IEnumerable<IPosition> Positions { get; set; }
    }
}