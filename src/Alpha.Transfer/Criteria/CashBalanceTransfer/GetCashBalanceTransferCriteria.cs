namespace Alpha.Transfer.Criteria.CashBalanceTransfer
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetCashBalanceTransferCriteria : CriteriaBase
    {
        public GetCashBalanceTransferCriteria(int custId, bool isDirectMember)
        {
            CustId = custId;
            IsDirectMember = isDirectMember;
        }

        public int CustId { get; }

        [SpParam(Ignored = true)]
        public bool IsDirectMember { get; }

        public override string GetSettingKey()
            => IsDirectMember ? "SiteDB_Age_Transfer_CashBalance_DirectMember_Sel" : "SiteDB_Age_Transfer_CashBalance_Sel";

        public override bool IsValid() => CustId > 0;
    }
}
