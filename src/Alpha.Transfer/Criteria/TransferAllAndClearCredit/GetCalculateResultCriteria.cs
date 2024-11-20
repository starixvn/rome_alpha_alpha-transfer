namespace Alpha.Transfer.Criteria.TransferAllAndClearCredit
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetCalculateResultCriteria : CriteriaBase
    {
        public GetCalculateResultCriteria(
            int custId,
            string username,
            string language,
            bool isAllDayBalanceTransfer,
            string downlineNetPosition,
            int downlineCustId)
        {
            CustId = custId;
            Username = username;
            Language = language;
            IsAllDayBalanceTransfer = isAllDayBalanceTransfer;
            DownlineCustId = downlineCustId;
            DownlineNetPosition = downlineNetPosition;
        }

        public string DownlineNetPosition { get; set; }

        public int DownlineCustId { get; set; }

        public int CustId { get; set; }

        [SpParam(SqlParamName = "DownlineUsername")]
        public string Username { get; set; }

        public string Language { get; set; }

        [SpParam(SqlParamName = "IsAllDayTransfer")]
        public bool IsAllDayBalanceTransfer { get; set; }

        public override string GetSettingKey()
            => "SiteDB_Age_Transfer_Credit_All_Sel";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username) && DownlineCustId > 0;
    }
}
