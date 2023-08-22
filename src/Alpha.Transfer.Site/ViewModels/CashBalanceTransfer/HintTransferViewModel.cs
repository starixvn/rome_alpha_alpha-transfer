namespace Alpha.Transfer.Site.ViewModels.CashBalanceTransfer
{
    public class HintTransferViewModel
    {
        public HintTransferViewModel(
            string allowTransfer,
            bool isAlldayTransfer = true,
            bool isEnabledAutoTransfer = false)
        {
            AllowTransfer = allowTransfer;
            IsAlldayTransfer = isAlldayTransfer;
            IsEnabledAutoTransfer = isEnabledAutoTransfer;
        }

        public string AllowTransfer { get; }

        public bool IsAlldayTransfer { get; }

        public bool IsEnabledAutoTransfer { get; }
    }
}