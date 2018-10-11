namespace Alpha.Transfer.Site.ViewModels.TransferAllAndClearCredit
{
    using System.Collections.Generic;
    using AppResources;
    using Helpers;
    using Params.TransferAllAndClearCredit;
    using Requests.TransferAllAndClearCredit;
    using Transfer.Models;
    using Transfer.Models.TransferAllAndClearCredit;

    public class TransferResultViewModel
    {
        public TransferResultViewModel(
            TransferResultQuery query,
            GetTransferResultParam param,
            TransferResult transferResult)
        {
            Username = query.Username;
            IsClearCredit = query.IsClearCredit;
            CustLevel = query.CustLevel;
            IsInternal = query.IsInternal;

            if (IsInternal)
            {
                TransferSteps = param.TransferSteps;
                ClearCreditSteps = param.ClearCreditSteps;
                Conditions = new List<string>();
            }
            else
            {
                ErrCodeTransfer = transferResult.ErrCodeTransfer;
                ErrCodeClearCredit = transferResult.ErrCodeClearCredit;
                TransferSteps = !transferResult.SkipTransfer && transferResult.ErrCodeTransfer == 0
                               ? transferResult.TransferSteps
                               : param.TransferSteps;
                ClearCreditSteps = transferResult.ErrCodeClearCredit == 0 ? transferResult.ClearCreditSteps : param.ClearCreditSteps;

                ErrMsgTransfer = transferResult.ErrCodeTransfer == 0
                    ? Resources.TransferAllSuccess
                    : string.IsNullOrWhiteSpace(transferResult.ErrMsgTransfer) ? Resources.TransferAllUnsuccessfully
                    : transferResult.ErrMsgTransfer;
                ErrMsgClearCredit = transferResult.ErrCodeClearCredit == 0
                    ? Resources.ClearCreditSuccess
                    : string.IsNullOrWhiteSpace(transferResult.ErrMsgClearCredit) ? Resources.ClearCreditUnsuccessfully
                    : transferResult.ErrMsgClearCredit;
                SkipTransfer = transferResult.SkipTransfer;
                Conditions = transferResult.Conditions;
            }
        }

        public UserLevel CustLevel { get; }

        public string Username { get; }

        public bool IsClearCredit { get; }

        public int ErrCodeTransfer { get; set; }

        public string ErrMsgTransfer { get; set; }

        public int ErrCodeClearCredit { get; set; }

        public string ErrMsgClearCredit { get; set; }

        public bool IsTransferSuccess => ErrCodeTransfer == 0;

        public bool IsClearCreditSuccess => ErrCodeClearCredit == 0;

        public bool SkipTransfer { get; }

        public bool IsInternal { get; }

        public IEnumerable<TransferStep> TransferSteps { get; }

        public IEnumerable<ClearCreditStep> ClearCreditSteps { get; }

        public IEnumerable<string> Conditions { get; }

        public string TransferStepsJson => JsonHelper.ToJsonFunc(TransferSteps).Replace("\"", "'");

        public string ClearCreditStepsJson => JsonHelper.ToJsonFunc(ClearCreditSteps).Replace("\"", "'");

        public string GetTransferStepName(TransferStep step)
            => string.Format(Resources.ResourceManager.GetString("TransferStep_" + StepKey(step.Id)), Username);

        public string GetClearCreditStepName(ClearCreditStep step)
            => string.Format(Resources.ResourceManager.GetString("ClearCreditStep_" + StepKey(step.Id)), Username);

        private string StepKey(byte id)
            => $"{CustLevel.DisplayName}_{Enumeration.FromValue<UserLevel>(id).DisplayName}";

        public string DirectDownlineText
            => Resources.ResourceManager.GetString(CustLevel.GetDirectDownLineLevel().DisplayName);
    }
}