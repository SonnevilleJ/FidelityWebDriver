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

        public AccountType AccountType { get; private set; }
        public double MostRecentValue { get; private set; }
        public string Name { get; private set; }
        public string AccountNumber { get; private set; }
    }
}