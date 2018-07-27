namespace Alpha.Transfer.Criteria.TransferAllAndClearCredit
{
    using Fanex.Data.Repository;

    public class GetClearCreditStepsCriteria : CriteriaBase
    {
        public GetClearCreditStepsCriteria(string username)
        {
            Username = username;
        }

        public string Username { get; set; }

        public override string GetSettingKey()
            => "Age_CustomerSelTransferSteps";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username);
    }
}
