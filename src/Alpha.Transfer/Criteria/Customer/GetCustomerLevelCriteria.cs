using Fanex.Data.Repository;

namespace Alpha.Transfer.Criteria.Customer
{
    internal class GetCustomerLevelCriteria : CriteriaBase
    {
        public GetCustomerLevelCriteria(string username, int uplineId)
        {
            Username = username;
            UplineId = uplineId;
        }

        public string Username { get; }

        public int UplineId { get; }

        public override string GetSettingKey() => "Age_Rpt_GetCustomerInfo";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(Username) && UplineId > 0;
    }
}
