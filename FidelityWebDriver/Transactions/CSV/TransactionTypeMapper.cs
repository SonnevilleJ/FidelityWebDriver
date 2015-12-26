using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Transactions.CSV
{
    public class TransactionTypeMapper : ITransactionTypeMapper
    {
        public TransactionType Map(string trimmedText)
        {
            if (trimmedText.Contains("YOU BOUGHT"))
                return TransactionType.Buy;
            if (trimmedText.Contains("YOU SOLD"))
                return TransactionType.Sell;
            if (trimmedText.Contains("REINVESTMENT"))
                return TransactionType.DividendReinvestment;
            if (trimmedText.Contains("DIVIDEND RECEIVED") ||
                trimmedText.Contains("CAP GAIN"))
                return TransactionType.DividendReceipt;
            if (trimmedText.Contains("Electronic Funds Transfer Received") ||
                (trimmedText.Contains("TRANSFERRED FROM") && trimmedText.Contains("TO BROKERAGE OPTION")) ||
                trimmedText.Contains("PARTIC CONTR"))
                return TransactionType.Deposit;
            return TransactionType.Unknown;
        }
    }
}