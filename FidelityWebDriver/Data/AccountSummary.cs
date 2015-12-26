namespace Sonneville.FidelityWebDriver.Data
{
    public class AccountSummary : IAccountSummary
    {
        public AccountSummary(string accountNumber, AccountType accountType, string accountName, double value)
        {
            AccountNumber = accountNumber;
            AccountType = accountType;
            Name = accountName;
            MostRecentValue = value;
        }

        public AccountType AccountType { get; }
        public double MostRecentValue { get; }
        public string Name { get; }
        public string AccountNumber { get; }
    }
}