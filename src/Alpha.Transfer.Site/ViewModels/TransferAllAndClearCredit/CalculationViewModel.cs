namespace Alpha.Transfer.Site.ViewModels.TransferAllAndClearCredit
{
    using System.Collections.Generic;
    using AppResources;
    using Helpers;
    using Requests.TransferAllAndClearCredit;
    using Transfer.Models;
    using Transfer.Models.TransferAllAndClearCredit;
    using Web.Helpers;
    using System.Linq;

    public class CalculationViewModel
    {
        public CalculationViewModel(
            CalculationQuery query,
            TransferResult calculationResult)
        {
            Username = query.Username;
            CustLevel = query.CustLevel;
            TransferSteps = calculationResult.TransferSteps;
            ClearCreditSteps = calculationResult.ClearCreditSteps;
        }

        public UserLevel CustLevel { get; }

        public string Username { get; }

        public IEnumerable<TransferStep> TransferSteps { get; }

        public IEnumerable<ClearCreditStep> ClearCreditSteps { get; }

        public string TransferStepsJson => JsonHelper.ToJsonFunc(TransferSteps).Replace("\"", "'");

        public string ClearCreditStepsJson => JsonHelper.ToJsonFunc(ClearCreditSteps).Replace("\"", "'");

        public bool IsShowCalculation => !string.IsNullOrWhiteSpace(Username);

        public string GetOutstandingText(decimal? outstanding)
            => outstanding.HasValue ? outstanding.Value.ToString($"#,##0.00") : "N/A";

        public string UsernameAutoSuggestionPath
            => MicroSites.OldMainUrl("~/_GlobalResources/Handlers/QueryUserName.ashx");

        public string GetTransferStepName(TransferStep step)
            => string.Format(Resources.ResourceManager.GetString("TransferStep_" + StepKey(step.Id)), Username);

        public string GetClearCreditStepName(ClearCreditStep step)
            => string.Format(Resources.ResourceManager.GetString("ClearCreditStep_" + StepKey(step.Id)), Username);

        private string StepKey(byte id)
            => $"{CustLevel.DisplayName}_{Enumeration.FromValue<UserLevel>(id).DisplayName}";

        public string DirectDownlineText
            => Resources.ResourceManager.GetString(CustLevel.GetDirectDownLineLevel().DisplayName);

        public bool CanClearCredit() => ClearCreditSteps.Any(s => s.ClearableCredit != 0);

        public bool CanTransfer() => TransferSteps.Any(s => s.TransferableAmount != 0);
    }
}