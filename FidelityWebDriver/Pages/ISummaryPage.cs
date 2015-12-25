namespace Sonneville.FidelityWebDriver.Pages
{
    public interface ISummaryPage : IPage
    {
        double GetBalanceOfAllAccounts();

        double GetGainLossAmount();

        double GetGainLossPercent();

        IPositionsPage GoToPositionsPage();

        IActivityPage GoToActivityPage();
    }
}