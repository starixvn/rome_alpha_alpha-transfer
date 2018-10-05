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
            string allowedTotalBalanceTransferSuperIds,
            bool isAllDayBalanceTransfer)
        {
            CustId = custId;
            Username = username;
            Language = language;
            AllowedTotalBalanceTransferSuperIds = allowedTotalBalanceTransferSuperIds;
            IsAllDayBalanceTransfer = isAllDayBalanceTransfer;
        }

        public int CustId { get; set; }

        [SpParam(SqlParamName = "DownlineUsername")]
        public string Username { get; set; }

        public string Language { get; set; }

        [SpParam(SqlParamName = "SuperIDsTransferTotal")]
        public string AllowedTotalBalanceTransferSuperIds { get; set; }

        [SpParam(SqlParamName = "IsAllDayTransfer")]
        public bool IsAllDayBalanceTransfer { get; set; }

        public override string GetSettingKey()
            => "Age_Transfer_Credit_All_Sel";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username);
    }
}
