namespace Alpha.Transfer.Criteria.CashBalanceTransfer
{
    using Fanex.Data.Repository;

    public class GetCashBalanceTransferCriteria : CriteriaBase
    {
        public GetCashBalanceTransferCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Age_Transfer_CashBalance_Sel";

        public override bool IsValid() => CustId > 0;
    }
}
