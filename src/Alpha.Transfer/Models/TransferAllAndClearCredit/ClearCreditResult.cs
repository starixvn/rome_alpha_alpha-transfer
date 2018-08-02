namespace Alpha.Transfer.Models.TransferAllAndClearCredit
{
    using System.Collections.Generic;

    public class ClearCreditResult
    {
        public ClearCreditResult(IEnumerable<ClearCreditStep> steps, int errCode, string errMsg)
        {
            Steps = steps;
            ErrCode = errCode;
            ErrMsg = errMsg;
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public IEnumerable<ClearCreditStep> Steps { get; }
    }
}
