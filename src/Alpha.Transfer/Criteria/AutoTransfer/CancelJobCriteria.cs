namespace Alpha.Transfer.Criteria.AutoTransfer
{
    using Fanex.Data.Repository;

    public class CancelJobCriteria : NonQueryCommand
    {
        public CancelJobCriteria(
            int fromCustId,
            int toCustId,
            string viewStatus,
            string language,
            string ip)
        {
            FromCustId = fromCustId;
            ToCustId = toCustId;
            ViewStatus = viewStatus;

            Language = language;
            IP = ip;

            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int FromCustId { get; }

        public int ToCustId { get; }

        public string ViewStatus { get; }

        public int Creator => FromCustId;

        public string Language { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "SiteDB_Age_ClearBalanceCust_ProcessCancel";

        public override bool IsValid() => FromCustId > 0 && ToCustId > 0;
    }
}
