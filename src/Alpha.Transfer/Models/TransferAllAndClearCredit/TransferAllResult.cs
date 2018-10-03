namespace Alpha.Transfer.Models.TransferAllAndClearCredit
{
    using System.Collections.Generic;
    using System.Linq;

    public class TransferAllResult
    {
        public TransferAllResult()
        {
            Steps = Enumerable.Empty<TransferStep>();
            ErrCode = 0;
            ErrMsg = string.Empty;
            Conditions = Enumerable.Empty<string>();
        }

        public TransferAllResult(IEnumerable<TransferStep> steps, int errCode, string errMsg, IEnumerable<string> conditions)
        {
            Steps = steps;
            ErrCode = errCode;
            ErrMsg = errMsg;
            Conditions = conditions;
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public IEnumerable<TransferStep> Steps { get; }

        public IEnumerable<string> Conditions { get; }

        public bool IsSuccess => ErrCode == 0;
    }
}
