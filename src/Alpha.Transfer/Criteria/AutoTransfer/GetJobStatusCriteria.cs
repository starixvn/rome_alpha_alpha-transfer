namespace Alpha.Transfer.Criteria.AutoTransfer
{
    using Fanex.Data.Repository;

    public class GetJobStatusCriteria : NonQueryCommand
    {
        public GetJobStatusCriteria(int fromCustId, int toCustId, bool isAllowAutoTransfer)
        {
            FromCustId = fromCustId;
            ToCustId = toCustId;
            IsAllowAutoTransfer = isAllowAutoTransfer;
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int FromCustId { get; }

        public int ToCustId { get; }

        public bool IsAllowAutoTransfer { get; }

        public bool IsAutoJobRunning { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "SiteDB_Age_ClearBalanceCust_Job_StatusCheck";

        public override bool IsValid() => true;
    }
}
