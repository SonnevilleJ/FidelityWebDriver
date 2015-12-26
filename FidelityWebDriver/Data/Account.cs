namespace Sonneville.FidelityWebDriver.Data
{
    public class Account : IAccount
    {
        public Account(string accountNumber, AccountType accountType, string accountName, double value)
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