namespace Sonneville.FidelityWebDriver.Data
{
    public interface IAccount
    {
        AccountType AccountType { get; }
        double MostRecentValue { get; }
        string Name { get; }
        string AccountNumber { get; }
    }
}