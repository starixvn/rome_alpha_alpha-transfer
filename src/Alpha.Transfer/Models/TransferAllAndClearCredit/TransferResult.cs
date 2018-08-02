namespace Alpha.Transfer.Models.TransferAllAndClearCredit
{
    using System.Collections.Generic;
    using System.Linq;

    public class TransferResult
    {
        public TransferResult()
        {
            TransferSteps = Enumerable.Empty<TransferStep>();
            ClearCreditSteps = Enumerable.Empty<ClearCreditStep>();
        }

        public TransferResult(
            IEnumerable<TransferStep> transferSteps,
            IEnumerable<ClearCreditStep> clearCreditSteps)
        {
            TransferSteps = transferSteps;
            ClearCreditSteps = clearCreditSteps;
        }

        public TransferResult(TransferAllResult transferAllResult, ClearCreditResult clearCreditResult, bool skipTransfer)
        {
            TransferSteps = transferAllResult.Steps;
            ClearCreditSteps = clearCreditResult.Steps;
            ErrCodeTransfer = transferAllResult.ErrCode;
            ErrMsgTransfer = transferAllResult.ErrMsg;
            ErrCodeClearCredit = clearCreditResult.ErrCode;
            ErrMsgClearCredit = clearCreditResult.ErrMsg;
            SkipTransfer = skipTransfer;
        }

        public int ErrCodeTransfer { get; set; }

        public string ErrMsgTransfer { get; set; }

        public int ErrCodeClearCredit { get; set; }

        public string ErrMsgClearCredit { get; set; }

        public bool SkipTransfer { get; set; }

        public IEnumerable<TransferStep> TransferSteps { get; }

        public IEnumerable<ClearCreditStep> ClearCreditSteps { get; }
    }
}
