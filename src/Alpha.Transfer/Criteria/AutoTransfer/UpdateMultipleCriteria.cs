namespace Alpha.Transfer.Criteria.CashBalanceTransfer
{
    using Alpha.Transfer.Constants;
    using Fanex.Data.Repository;

    public class UpdateMultipleCriteria : NonQueryCommand
    {
        public UpdateMultipleCriteria(
            int fromCustId,
            int toCustId,
            bool isAutoJobRunning,
            string language,
            string ip,
            string state)
        {
            FromCustId = fromCustId;
            ToCustId = toCustId;
            IsAutoJobRunning = isAutoJobRunning;

            IsAllowAutoTransfer = string.Equals(AutoTransferConstant.On, state, System.StringComparison.InvariantCultureIgnoreCase);
            Language = language;
            IP = ip;
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int FromCustId { get; }

        public int ToCustId { get; }

        public bool IsAutoJobRunning { get; }

        public int Creator => FromCustId;

        public string Language { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public bool IsAllowAutoTransfer { get; }

        public override string GetSettingKey() => "Age_ClearBalanceCust_SettingUpdate";

        public override bool IsValid() =>
            FromCustId > 0 &&
            ToCustId > 0 &&
            !string.IsNullOrEmpty(IP);
    }
}
