namespace Alpha.Transfer.Criteria.CashBalanceTransfer
{
    using Alpha.Transfer.Constants;
    using Fanex.Data.Repository;

    public class GetJobStatusCriteria : NonQueryCommand
    {
        public GetJobStatusCriteria(int fromCustId, int toCustId, string status)
        {
            FromCustId = fromCustId;
            ToCustId = toCustId;
            IsAllowAutoTransfer = string.Equals(AutoTransferConstant.On, status, System.StringComparison.InvariantCultureIgnoreCase);
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int FromCustId { get; }

        public int ToCustId { get; }

        public bool IsAllowAutoTransfer { get; }

        public bool IsAutoJobRunning { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_ClearBalanceCust_Job_StatusCheck";

        public override bool IsValid() => true;
    }
}
