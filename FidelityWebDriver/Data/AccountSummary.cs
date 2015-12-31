namespace Sonneville.FidelityWebDriver.Data
{
    public class AccountSummary : IAccountSummary
    {
        public AccountType AccountType { get; set; }

        public double MostRecentValue { get; set; }

        public string Name { get; set; }

        public string AccountNumber { get; set; }
    }
}