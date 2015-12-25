using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.CSV
{
    public interface ITransactionTypeMapper
    {
        TransactionType Map(string trimmedText);
    }
}