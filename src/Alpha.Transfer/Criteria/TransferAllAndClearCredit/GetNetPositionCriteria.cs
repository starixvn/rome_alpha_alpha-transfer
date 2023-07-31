namespace Alpha.Transfer.Criteria.TransferAllAndClearCredit
{
    using Fanex.Data.Repository;

    public class GetNetPositionCriteria : CriteriaBase
    {
        public GetNetPositionCriteria(
            int custId,
            byte roleId)
        {
            CustId = custId;
            RoleId = roleId;
        }

        public int CustId { get; set; }

        public byte RoleId { get; set; }

        public override string GetSettingKey()
            => "SiteDB_Age_GetCustomerNetPosition";

        public override bool IsValid()
            => RoleId > 0 && CustId > 0;
    }
}
