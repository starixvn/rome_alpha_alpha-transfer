namespace Alpha.Transfer.Criteria.TransferAllAndClearCredit
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ClearCreditCriteria : CriteriaBase
    {
        public ClearCreditCriteria(
            int custId,
            int userId,
            int subAccId,
            string username,
            string ip,
            string language)
        {
            CustId = custId;
            UserId = userId;
            SubAccId = subAccId;
            Username = username;
            IP = ip;
            Language = language;
        }

        public int CustId { get; set; }

        public int UserId { get; set; }

        public int SubAccId { get; set; }

        [SpParam(SqlParamName = "DownlineUsername")]
        public string Username { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Age_ClearCredit_All_Ins";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username);
    }
}
