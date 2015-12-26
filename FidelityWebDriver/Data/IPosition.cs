namespace Sonneville.FidelityWebDriver.Data
{
    public interface IPosition
    {
        string Ticker { get; }

        string Description { get; }

        bool IsCore { get; }

        bool IsMargin { get; }

        decimal LastPrice { get; }

        decimal TotalGainDollar { get; }

        decimal TotalGainPercent { get; }

        decimal CurrentValue { get; }

        decimal Quantity { get; }

        decimal CostBasisPerShare { get; }

        decimal CostBasisTotal { get; }
    }
}