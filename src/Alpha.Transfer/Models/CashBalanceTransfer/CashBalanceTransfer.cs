namespace Alpha.Transfer.Models.CashBalanceTransfer
{
    public class CashBalanceTransfer
    {
        public CashBalanceTransfer()
        {
        }

        public CashBalanceTransfer(
            int custId,
            byte roleId,
            string userName,
            decimal uplineCashBalance,
            decimal cashBalance,
            decimal outstanding,
            decimal givenCredit,
            decimal pendingTransfer3rd)
        {
            CustId = custId;
            UserName = userName;
            RoleId = roleId;
            UplineCashBalance = uplineCashBalance;
            CashBalance = cashBalance;
            Outstanding = outstanding;
            GivenCredit = givenCredit;
            PendingTransfer3rd = pendingTransfer3rd;
        }

        public int CustId { get; set; }

        public string UserName { get; set; }

        public byte RoleId { get; set; }

        public decimal UplineCashBalance { get; set; }

        public decimal CashBalance { get; set; }

        public decimal Outstanding { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal PendingTransfer3rd { get; set; }
    }
}
