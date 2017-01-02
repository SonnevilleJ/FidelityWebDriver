using Sonneville.FidelityWebDriver.Positions;
using Sonneville.FidelityWebDriver.Transactions;

namespace Sonneville.FidelityWebDriver.Navigation
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