using System.Collections.Generic;
using System.Linq;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Transactions.CSV
{
    public class TransactionTypeMapper : ITransactionTypeMapper
    {
        private readonly Dictionary<TransactionType, string> _forwardMappings = new Dictionary<TransactionType, string>
        {
            {TransactionType.Buy, "YOU BOUGHT           PROSPECTUS UNDER    SEPARATE COVER"},
            {TransactionType.Sell, "YOU SOLD             EXCHANGE"},
            {TransactionType.DividendReinvestment, "REINVESTMENT"},
            {TransactionType.DividendReceipt, "DIVIDEND RECEIVED"},
            {TransactionType.ShortTermCapGain, "SHORT-TERM CAP GAIN"},
            {TransactionType.LongTermCapGain, "LONG-TERM CAP GAIN"},
            {TransactionType.Deposit, "Electronic Funds Transfer Received"},
            {TransactionType.DepositBrokeragelink, "TRANSFERRED FROM     TO BROKERAGE OPTION"},
            {TransactionType.DepositHSA, "PARTIC CONTR CURRENT PARTICIPANT CUR YR"},
            {TransactionType.Withdrawal, "Electronic Funds Transfer Paid"},
        };

        private readonly Dictionary<TransactionType, string> _reverseMappings = new Dictionary<TransactionType, string>
        {
            {TransactionType.Buy, "YOU BOUGHT"},
            {TransactionType.Sell, "YOU SOLD"},
            {TransactionType.DividendReinvestment, "REINVESTMENT"},
            {TransactionType.DividendReceipt, "DIVIDEND RECEIVED"},
            {TransactionType.ShortTermCapGain, "SHORT-TERM CAP GAIN"},
            {TransactionType.LongTermCapGain, "LONG-TERM CAP GAIN"},
            {TransactionType.Deposit, "Electronic Funds Transfer Received"},
            {TransactionType.DepositBrokeragelink, "TRANSFERRED FROM     TO BROKERAGE OPTION"},
            {TransactionType.DepositHSA, "PARTIC CONTR"},
            {TransactionType.Withdrawal, "Electronic Funds Transfer Paid"},
        };

        public TransactionType MapValue(string trimmedText)
        {
            return _reverseMappings.SingleOrDefault(mapping => trimmedText.Contains(mapping.Value)).Key;
        }

        public string MapKey(TransactionType transactionType)
        {
            return _forwardMappings[transactionType];
        }
    }
}