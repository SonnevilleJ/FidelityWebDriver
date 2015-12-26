using System;
using System.Collections.Generic;
using System.Globalization;
using OpenQA.Selenium;
using Sonneville.FidelityWebDriver.Data;

namespace Sonneville.FidelityWebDriver.Positions
{
    public class AccountDetailsExtractor : IAccountDetailsExtractor
    {
        private readonly IPositionDetailsExtractor _positionDetailsExtractor;

        public AccountDetailsExtractor(IPositionDetailsExtractor positionDetailsExtractor)
        {
            _positionDetailsExtractor = positionDetailsExtractor;
        }

        public IEnumerable<IAccountDetails> ExtractAccountDetails(IWebDriver webDriver)
        {
            var table = webDriver.FindElement(By.ClassName("p-positions-tbody"));
            var tableRows = table.FindElements(By.TagName("tr"));

            var collectingPositionRows = false;

            List<IWebElement> positionRows = null;
            AccountDetails accountDetails = null;

            var results = new List<IAccountDetails>();
            foreach (var tableRow in tableRows)
            {
                Console.WriteLine("Iterating rows...");
                if (collectingPositionRows)
                {
                    if (IsPositionRow(tableRow))
                    {
                        Console.WriteLine("Found position row!");
                        positionRows.Add(tableRow);
                        continue;
                    }
                    if (IsTotalRow(tableRow))
                    {
                        Console.WriteLine("Found total row!");
                        var rawPendingActivityText = tableRow
                            .FindElement(By.ClassName("magicgrid--total-pending-activity-link"))
                            .FindElement(By.ClassName("value"))
                            .Text;
                        accountDetails.PendingActivity = decimal.Parse(rawPendingActivityText, NumberStyles.Any);

                        var totalGainSpans = tableRow.FindElements(By.ClassName("magicgrid--stacked-data-value"));
                        accountDetails.TotalGainDollar = decimal.Parse(totalGainSpans[0].Text.Trim(), NumberStyles.Any);
                        var trimmedPercentString = totalGainSpans[1].Text.Trim('%');
                        accountDetails.TotalGainPercent = decimal.Parse(trimmedPercentString, NumberStyles.Any)/100m;
                    }
                }
                if (IsNewAccountRow(tableRow))
                {
                    Console.WriteLine("Found new account!");
                    if (accountDetails != null)
                    {
                        results.Add(PrepareAccountDetails(accountDetails, positionRows));
                    }
                    positionRows = new List<IWebElement>();
                    accountDetails = new AccountDetails
                    {
                        Name = ExtractAccountName(tableRow),
                        AccountNumber = ExctractAccountNumber(tableRow)
                    };
                    collectingPositionRows = true;
                }
            }
            results.Add(PrepareAccountDetails(accountDetails, positionRows));
            return results;
        }

        private AccountDetails PrepareAccountDetails(AccountDetails accountDetails,
            IEnumerable<IWebElement> positionRows)
        {
            accountDetails.Positions = _positionDetailsExtractor.ExtractPositionDetails(positionRows);
            return accountDetails;
        }

        private static string ExctractAccountNumber(IWebElement tableRow)
        {
            return tableRow.FindElement(By.ClassName("magicgrid--account-title-description")).Text;
        }

        private static string ExtractAccountName(IWebElement tableRow)
        {
            return tableRow.FindElement(By.ClassName("magicgrid--account-title-text")).Text
                .Replace("-", "").Trim();
        }

        private bool IsTotalRow(IWebElement tableRow)
        {
            var classes = tableRow.GetAttribute("class");
            return !string.IsNullOrWhiteSpace(classes)
                   && (classes.Contains("magicgrid--total-row"));
        }

        private bool IsPositionRow(IWebElement tableRow)
        {
            var classes = tableRow.GetAttribute("class");
            return !string.IsNullOrWhiteSpace(classes)
                   && (classes.Contains("normal-row") || classes.Contains("content-row"));
        }

        private bool IsNewAccountRow(IWebElement tableRow)
        {
            var classes = tableRow.GetAttribute("class");
            return !string.IsNullOrWhiteSpace(classes)
                   && classes.Trim().Contains("magicgrid--account-title-row");
        }
    }
}