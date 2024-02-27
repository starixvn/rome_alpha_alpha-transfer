namespace Alpha.Transfer.Criteria.AutoTransfer
{
    using Fanex.Data.Repository;

    public class UpdateStatusCriteria : NonQueryCommand
    {
        public UpdateStatusCriteria(
            int fromCustId,
            int toCustId,
            string autoTransferSchedule,
            bool isAutoJobRunning,
            bool isAllowAutoTransfer,
            string language,
            string ip,
            string oldTransferOption,
            int subAccId)
        {
            FromCustId = fromCustId;
            ToCustId = toCustId;
            AutoTransferSchedule = autoTransferSchedule;
            IsAutoJobRunning = isAutoJobRunning;

            IsAllowAutoTransfer = isAllowAutoTransfer;
            Language = language;
            IP = ip;

            Old_TransferOption = oldTransferOption;
            SubAccId = subAccId;
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int FromCustId { get; }

        public int ToCustId { get; }

        public bool IsAutoJobRunning { get; }

        public string AutoTransferSchedule { get; }

        public int SubAccId { get; }

        public int Creator => FromCustId;

        public string Language { get; set; }

        public string IP { get; set; }

        public string Old_TransferOption { get; set; }

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
