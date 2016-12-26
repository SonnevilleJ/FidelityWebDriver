using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Transactions.CSV
{
    public interface ITransactionTypeMapper
    {
        TransactionType MapValue(string trimmedText);
    }
}