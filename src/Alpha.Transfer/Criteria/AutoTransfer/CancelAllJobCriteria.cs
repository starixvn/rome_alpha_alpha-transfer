using Fanex.Data.Repository;

namespace Alpha.Transfer.Criteria.AutoTransfer
{
    public class CancelAllJobCriteria : NonQueryCommand
    {
        public CancelAllJobCriteria(int fromCustId, string language, string ip)
        {
            FromCustId = fromCustId;
            Language = language;
            IP = ip;

            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int FromCustId { get; }

        public string Language { get; set; }

        public string IP { get; set; }

        public int Creator => FromCustId;

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "SiteDB_Age_ClearBalanceCust_ProcessCancel_All";

        public override bool IsValid() => FromCustId > 0;
    }
}
