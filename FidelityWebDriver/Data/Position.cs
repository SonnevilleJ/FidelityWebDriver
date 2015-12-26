namespace Sonneville.FidelityWebDriver.Data
{
    public class Position : IPosition
    {
        public Position(string ticker, string description, bool isCore, bool isMargin, decimal lastPrice,
            decimal totalGainDollar, decimal totalGainPercent, decimal currentValue, decimal quantity,
            decimal costBasisPerShare, decimal costBasisTotal)
        {
            Ticker = ticker;
            Description = description;
            IsCore = isCore;
            IsMargin = isMargin;
            LastPrice = lastPrice;
            TotalGainDollar = totalGainDollar;
            TotalGainPercent = totalGainPercent;
            CurrentValue = currentValue;
            Quantity = quantity;
            CostBasisPerShare = costBasisPerShare;
            CostBasisTotal = costBasisTotal;
        }

        public string Ticker { get; }

        public string Description { get; }

        public bool IsCore { get; }

        public bool IsMargin { get; }

        public decimal LastPrice { get; }

        public decimal TotalGainDollar { get; }

        public decimal TotalGainPercent { get; }

        public decimal CurrentValue { get; }

        public decimal Quantity { get; }

        public decimal CostBasisPerShare { get; }

        public decimal CostBasisTotal { get; }
    }
}