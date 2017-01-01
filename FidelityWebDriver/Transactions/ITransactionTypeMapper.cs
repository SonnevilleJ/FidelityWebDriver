using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Transactions
{
    public interface ITransactionTypeMapper
    {
        TransactionType ClassifyDescription(string description);
    }
}