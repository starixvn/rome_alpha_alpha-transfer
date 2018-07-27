namespace Alpha.Transfer.Criteria.TransferAllAndClearCredit
{
    using Fanex.Data.Repository;

    public class GetTransferResultCriteria : CriteriaBase
    {
        public GetTransferResultCriteria(string username, bool isClearCredit)
        {
            Username = username;
            IsClearCredit = isClearCredit;
        }

        public string Username { get; set; }

        public bool IsClearCredit { get; set; }

        public override string GetSettingKey()
            => "Age_CustomerTransferAll";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username);
    }
}
