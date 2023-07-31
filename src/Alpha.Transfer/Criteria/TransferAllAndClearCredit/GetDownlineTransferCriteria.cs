namespace Alpha.Transfer.Criteria.TransferAllAndClearCredit
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetDownlineTransferCriteria : NonQueryCommand
    {
        public GetDownlineTransferCriteria(
            int custId,
            string downlineUserName)
        {
            CustId = custId;
            Username = downlineUserName;
        }

        public int CustId { get; set; }

        [SpParam(SqlParamName = "DownlineUsername")]
        public string Username { get; set; }

        public int DownlineCustId { get; set; }

        public byte DownlineRoleId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Age_Transfer_Credit_All_SelDownlineInfo";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username) && CustId > 0;
    }
}
