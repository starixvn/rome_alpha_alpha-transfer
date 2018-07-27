namespace Alpha.Transfer.Criteria
{
    using Fanex.Data.Repository;

    public class GetTransferOptionCriteria : NonQueryCommand
    {
        public GetTransferOptionCriteria(int custId)
        {
            CustId = custId;
            TransferOption = string.Empty;
        }

        public int CustId { get; }

        public string TransferOption { get; set; }

        public override string GetSettingKey() => "Acc_CustomerSelTransferOption";

        public override bool IsValid() => CustId > 0;
    }
}
