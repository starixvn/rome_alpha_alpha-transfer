namespace Alpha.Transfer.Criteria.CashBalanceTransfer
{
    using Fanex.Data.Repository;

    public class CancelJobCriteria : NonQueryCommand
    {
        public CancelJobCriteria(int fromCustId, int toCustId, string languageKey, string ip)
        {
            FromCustId = fromCustId;
            ToCustId = toCustId;
            Language = languageKey;
            IP = ip;
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int FromCustId { get; }

        public int ToCustId { get; }

        public int Creator => FromCustId;

        public string Language { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_ClearBalanceCust_Cancel";

        public override bool IsValid() =>
            FromCustId > 0 &&
            ToCustId > 0 &&
            !string.IsNullOrEmpty(IP);
    }
}
