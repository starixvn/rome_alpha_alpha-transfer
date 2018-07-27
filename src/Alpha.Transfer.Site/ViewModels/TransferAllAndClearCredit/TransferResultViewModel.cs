namespace Alpha.Transfer.Site.ViewModels.TransferAllAndClearCredit
{
    using System.Collections.Generic;
    using AppResources;
    using Helpers;
    using Params.TransferAllAndClearCredit;
    using Requests.TransferAllAndClearCredit;
    using Transfer.Models.TransferAllAndClearCredit;
    using Web.Helpers;

    public class TransferResultViewModel
    {
        public TransferResultViewModel(
            TransferResultQuery query,
            GetTransferResultParam param,
            TransferResult transferResult)
        {
            Username = query.Username;
            IsClearCredit = query.IsClearCredit;

            IsSuccess = transferResult.IsSuccess;
            ErrorMessage = transferResult.ErrorMessage;

            TransferSteps = transferResult.IsSuccess ? transferResult.TransferSteps : param.TransferSteps;
            ClearCreditSteps = transferResult.IsSuccess ? transferResult.ClearCreditSteps : param.ClearCreditSteps;
            HasDataChanges = transferResult.HasDataChanged();
        }

        public string Username { get; }

        public bool IsClearCredit { get; }

        public bool IsSuccess { get; }

        public string ErrorMessage { get; }

        public bool HasDataChanges { get; }

        public IEnumerable<TransferStep> TransferSteps { get; }

        public IEnumerable<ClearCreditStep> ClearCreditSteps { get; }

        public string TransferStepsJson => JsonHelper.ToJsonFunc(TransferSteps);

        public string ClearCreditStepsJson => JsonHelper.ToJsonFunc(ClearCreditSteps);

        public string GetOutstandingText(decimal? outstanding)
            => outstanding.HasValue ? outstanding.Value.ToString($"#,##0.00") : "N/A";

        public string UsernameAutoSuggestionPath
            => MicroSites.OldMainUrl("~/_GlobalResources/Handlers/QueryUserName.ashx");

        public string MessageResult
            => HasDataChanges ? Resources.DataChangedWarning
            : IsSuccess ? Resources.TransferAllAndClearCreditSuccess
            : ErrorMessage;

        public string MessageType => HasDataChanges ? "warning" : IsSuccess ? "success" : "error";
    }
}