using Fanex.Data.Repository;

namespace Alpha.Transfer.Criteria
{
    public class CheckMultipleCustomesBelongingCriteria : NonQueryCommand
    {
        public CheckMultipleCustomesBelongingCriteria(int actorId, int subAccId, string custIds)
        {
            SessionCustId = actorId;
            SubAccountId = subAccId;
            CustId = custIds;
        }

        public int SessionCustId { get; }

        public int SubAccountId { get; }

        public string CustId { get; }

        public byte RoleId { get; set; }

        public bool IsActive { get; set; }

        public override string GetSettingKey() => "Age_CustomerSelCheckBelonging_MultiCust";

        public override bool IsValid() => SessionCustId > 0 && !string.IsNullOrWhiteSpace(CustId);
    }
}
