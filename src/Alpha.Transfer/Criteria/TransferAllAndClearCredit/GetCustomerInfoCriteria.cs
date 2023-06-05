namespace Alpha.Transfer.Criteria.TransferAllAndClearCredit
{
    using Fanex.Data.Repository;

    public class GetCustomerInfoCriteria : NonQueryCommand
    {
        public GetCustomerInfoCriteria(string username)
        {
            Username = username;
        }

        public string Username { get; set; }

        public int CustId { get; set; }

        public byte RoleId { get; set; }

        public override string GetSettingKey()
            => "Acc_CustomerSelInfoByUsername";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username);
    }
}
