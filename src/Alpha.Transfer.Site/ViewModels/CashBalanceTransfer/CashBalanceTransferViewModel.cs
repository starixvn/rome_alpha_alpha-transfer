namespace Alpha.Transfer.Site.ViewModels.CashBalanceTransfer
{
    using AppResources;
    using Transfer.Models;

    public class CashBalanceTransferViewModel
    {
        public CashBalanceTransferViewModel(
            decimal uplineCashBalance,
            int custId,
            string userName,
            UserLevel custUserLevel,
            decimal cashBalance,
            decimal outstanding,
            decimal givenCredit,
            decimal pendingTransfer3rd)
        {
            UplineCashBalance = uplineCashBalance;
            CustId = custId;
            UserName = userName;
            CustUserLevel = custUserLevel;
            CashBalance = cashBalance;
            Outstanding = outstanding;
            GivenCredit = givenCredit;

            TransferAmountFrom = CashBalance - Outstanding - pendingTransfer3rd;
            TransferAmountFrom = TransferAmountFrom > 0 ? TransferAmountFrom * -1 : 0;
            TransferAmountTo = UplineCashBalance > 0 ? UplineCashBalance : 0;

            DisableTransfer = TransferAmountFrom == 0 && TransferAmountTo == 0;

            if (DisableTransfer)
            {
                Hint = Resources.HintNotAvailableToTransfer;
            }
            else
            {
                string amountFrom = Format4Digits(TransferAmountFrom);
                string amountTo = Format4Digits(TransferAmountTo);

                Hint = string.Format(Resources.HintRangeTransfer, amountFrom, amountTo);
                AvailableRangeTransfer = string.Format(Resources.AvailableRangeTransfer, amountFrom, amountTo);
            }
        }

        public string UplineLevelName
        {
            get
            {
                return GetUserLevelDisplayName(CustUserLevel.GetDirectUplineLevel());
            }
        }

        public decimal UplineCashBalance { get; }

        public string CustLevelName
        {
            get
            {
                return GetUserLevelDisplayName(CustUserLevel);
            }
        }

        public int CustId { get; }

        public string UserName { get; }

        public UserLevel CustUserLevel { get; }

        public decimal CashBalance { get; }

        public decimal Outstanding { get; }

        public decimal GivenCredit { get; }

        public decimal TransferAmountFrom { get; }

        public decimal TransferAmountTo { get; }

        public string Hint { get; }

        public string AvailableRangeTransfer { get; }

        public bool DisableTransfer { get; }

        private static string Format4Digits(decimal value)
        {
            return value == 0 ? "0" : value.ToString("N4");
        }

        private static string GetUserLevelDisplayName(UserLevel userLevel)
        {
            if (userLevel.IsSuperLevel())
            {
                return Resources.Super;
            }

            if (userLevel.IsMasterLevel())
            {
                return Resources.Master;
            }

            if (userLevel.IsAgentLevel())
            {
                return Resources.Agent;
            }

            return Resources.Member;
        }
    }
}