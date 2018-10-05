namespace Alpha.Transfer.Criteria.TransferAllAndClearCredit
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetCalculateResultCriteria : CriteriaBase
    {
        public GetCalculateResultCriteria(int custId, string username, string language)
        {
            CustId = custId;
            Username = username;
        }

        public int CustId { get; set; }

        [SpParam(SqlParamName = "DownlineUsername")]
        public string Username { get; set; }

        public string Language { get; set; }

        public override string GetSettingKey()
            => "Age_Transfer_Credit_All_Sel";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username);
    }
}
