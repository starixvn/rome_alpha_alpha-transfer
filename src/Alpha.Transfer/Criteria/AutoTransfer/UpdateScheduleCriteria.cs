namespace Alpha.Transfer.Criteria.AutoTransfer
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class UpdateScheduleCriteria : NonQueryCommand
    {
        public UpdateScheduleCriteria(
            int custId,
            string custName,
            string autoTransferSchedule,
            int uplineId,
            int subAccId,
            string ip,
            string language,
            string oldTransferOption,
            bool isAutoJobRunning)
        {
            CustId = custId;
            CustName = custName;
            AutoTransferSchedule = autoTransferSchedule;

            UplineId = uplineId;
            SubAccId = subAccId;
            IP = ip;
            Language = language;
            Old_TransferOption = oldTransferOption;
            IsAutoJobRunning = isAutoJobRunning;

            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        [SpParam(SqlParamName = "ToCustId")]
        public int CustId { get; set; }

        [SpParam(Ignored = true)]
        public string CustName { get; set; }

        public string AutoTransferSchedule { get; set; }

        [SpParam(SqlParamName = "FromCustId")]
        public int UplineId { get; set; }

        public int SubAccId { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public string Old_TransferOption { get; set; }

        public bool IsAutoJobRunning { get; set; }

        public int Creator => UplineId;

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_AutoTransferSchedule_Upd";

        public override bool IsValid() => UplineId > 0 && CustId > 0;
    }
}
