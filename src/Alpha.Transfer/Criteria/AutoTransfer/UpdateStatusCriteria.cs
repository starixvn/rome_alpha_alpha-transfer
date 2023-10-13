using Alpha.Transfer.Constants;
using Alpha.Transfer.Helpers;
using Alpha.Transfer.Params.AutoTransfer;
using Fanex.Data.Repository;

namespace Alpha.Transfer.Criteria.AutoTransfer
{
    public class UpdateStatusCriteria : NonQueryCommand
    {
        public UpdateStatusCriteria(UpdateStatusParam param, bool isAutoJobRunning)
        {
            FromCustId = param.UplineId;
            ToCustId = param.CustId;
            AutoTransferSchedule = TransferConditionHelper.GetAutoTransferScheduleFromDates(param.Schedule);
            IsAutoJobRunning = isAutoJobRunning;

            IsAllowAutoTransfer = string.Equals(AutoTransferConstant.On, param.State, System.StringComparison.InvariantCultureIgnoreCase);
            Language = param.Language;
            IP = param.IP;
            ErrCode = 0;
            ErrMsg = string.Empty;
            SubAccId = param.SubAccId;
        }

        public int FromCustId { get; }

        public int ToCustId { get; }

        public bool IsAutoJobRunning { get; }

        public string AutoTransferSchedule { get; }

        public int SubAccId { get; }

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
