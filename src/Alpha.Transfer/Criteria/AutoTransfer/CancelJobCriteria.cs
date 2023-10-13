using Alpha.Transfer.Params.AutoTransfer;
using Fanex.Data.Repository;

namespace Alpha.Transfer.Criteria.AutoTransfer
{
    public class CancelJobCriteria : NonQueryCommand
    {
        public CancelJobCriteria(CancelJobParam param)
        {
            FromCustId = param.FromCustId;
            ToCustId = param.ToCustId;
            ViewStatus = param.ViewStatus;

            Language = param.Language;
            IP = param.IP;

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
