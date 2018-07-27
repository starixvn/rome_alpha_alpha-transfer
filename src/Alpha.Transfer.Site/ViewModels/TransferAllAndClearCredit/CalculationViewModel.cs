namespace Alpha.Transfer.Site.ViewModels.TransferAllAndClearCredit
{
    using System.Collections.Generic;
    using Helpers;
    using Requests.TransferAllAndClearCredit;
    using Transfer.Models.TransferAllAndClearCredit;
    using Web.Helpers;

    public class CalculationViewModel
    {
        public CalculationViewModel(
            CalculationQuery query,
            IEnumerable<TransferStep> transferSteps,
            IEnumerable<ClearCreditStep> clearCreditSteps)
        {
            Username = query.Username;
            TransferSteps = transferSteps;
            ClearCreditSteps = clearCreditSteps;
        }

        public string Username { get; }

        public IEnumerable<TransferStep> TransferSteps { get; }

        public IEnumerable<ClearCreditStep> ClearCreditSteps { get; }

        public string TransferStepsJson => JsonHelper.ToJsonFunc(TransferSteps);

        public string ClearCreditStepsJson => JsonHelper.ToJsonFunc(ClearCreditSteps);

        public bool IsShowCalculation => !string.IsNullOrWhiteSpace(Username);

        public string GetOutstandingText(decimal? outstanding)
            => outstanding.HasValue ? outstanding.Value.ToString($"#,##0.00") : "N/A";

        public string UsernameAutoSuggestionPath
            => MicroSites.OldMainUrl("~/_GlobalResources/Handlers/QueryUserName.ashx");
    }
}