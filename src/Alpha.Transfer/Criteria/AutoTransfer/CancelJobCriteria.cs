using Fanex.Data.Repository;

namespace Alpha.Transfer.Criteria.AutoTransfer
{
    public class CancelJobCriteria : NonQueryCommand
    {
        public CancelJobCriteria(int fromCustId, int toCustId, string viewStatus)
        {
            FromCustId = fromCustId;
            ToCustId = toCustId;
            ViewStatus = viewStatus;
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int FromCustId { get; }

        public int ToCustId { get; }

        public string ViewStatus { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_ClearBalanceCust_ProcessCancel";

        public override bool IsValid() => FromCustId > 0 && ToCustId > 0;
    }
}
