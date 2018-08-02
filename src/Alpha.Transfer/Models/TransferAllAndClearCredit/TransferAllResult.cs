namespace Alpha.Transfer.Models.TransferAllAndClearCredit
{
    using System.Collections.Generic;

    public class TransferAllResult
    {
        public TransferAllResult(IEnumerable<TransferStep> steps, int errCode, string errMsg)
        {
            Steps = steps;
            ErrCode = errCode;
            ErrMsg = errMsg;
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public IEnumerable<TransferStep> Steps { get; }

        public bool IsSuccess => ErrCode == 0;
    }
}
