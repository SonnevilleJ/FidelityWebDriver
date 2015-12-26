namespace Sonneville.FidelityWebDriver.Data
{
    public interface IAccountSummary
    {
        AccountType AccountType { get; }
        double MostRecentValue { get; }
        string Name { get; }
        string AccountNumber { get; }
    }
}