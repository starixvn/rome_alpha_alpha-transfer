using Alpha.Transfer.Helpers;
using Alpha.Transfer.Params.AutoTransfer;
using Fanex.Data;
using Fanex.Data.Repository;

namespace Alpha.Transfer.Criteria.AutoTransfer
{
    public class UpdateScheduleCriteria : NonQueryCommand
    {
        public UpdateScheduleCriteria(UpdateScheduleParam param)
        {
            CustId = param.CustId;
            CustName = param.CustName;
            AutoTransferSchedule = TransferConditionHelper.GetAutoTransferScheduleFromDates(param.Schedule);

            UplineId = param.UplineId;
            SubAccId = param.SubAccId;
            IP = param.IP;
            Language = param.Language;

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

        public int Creator => UplineId;

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_AutoTransferSchedule_Upd";

        public override bool IsValid() => UplineId > 0 && CustId > 0;
    }
}
