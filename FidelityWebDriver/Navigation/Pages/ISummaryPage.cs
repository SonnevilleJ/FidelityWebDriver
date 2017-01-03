namespace Sonneville.FidelityWebDriver.Navigation.Pages
{
    public interface ISummaryPage : IPage
    {
        double GetBalanceOfAllAccounts();

        double GetGainLossAmount();

        double GetGainLossPercent();

        void GoToPositionsPage();

        void GoToActivityPage();
    }
}