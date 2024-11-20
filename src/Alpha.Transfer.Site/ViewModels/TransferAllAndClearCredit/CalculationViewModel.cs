namespace Alpha.Transfer.Site.ViewModels.TransferAllAndClearCredit
{
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using Helpers;
    using Requests.TransferAllAndClearCredit;
    using Transfer.Models;
    using Transfer.Models.TransferAllAndClearCredit;

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
            Conditions = calculationResult.Conditions;
        }

        public UserLevel CustLevel { get; }

        public string Username { get; }

        public IEnumerable<TransferStep> TransferSteps { get; }

        public IEnumerable<ClearCreditStep> ClearCreditSteps { get; }

        public IEnumerable<string> Conditions { get; }

        public string TransferStepsJson => JsonHelper.ToJsonFunc(TransferSteps).Replace("\"", "'");

        public string ClearCreditStepsJson => JsonHelper.ToJsonFunc(ClearCreditSteps).Replace("\"", "'");

        public bool IsShowCalculation => !string.IsNullOrWhiteSpace(Username);

        public string GetTransferStepName(TransferStep step)
            => string.Format(Resources.ResourceManager.GetString("TransferStep_" + StepKey(step.Id)), Username);

        public string GetClearCreditStepName(ClearCreditStep step)
            => string.Format(Resources.ResourceManager.GetString("ClearCreditStep_" + StepKey(step.Id)), Username);

        private string StepKey(int id)
            => $"{CustLevel.DisplayName}_{(id > 0 ? Enumeration.FromValue<UserLevel>((byte)id).DisplayName : "DirectMember")}";

        public string DirectDownlineText
            => Resources.ResourceManager.GetString(CustLevel.GetDirectDownLineLevel().DisplayName);

        public bool CanClearCredit() => ClearCreditSteps.Any(s => s.ClearableCredit != 0);

        public bool CanTransfer() => TransferSteps.Any(s => s.TransferableAmount != 0);
    }
}