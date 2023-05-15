namespace Alpha.Transfer.Site.ViewModels.CashBalanceTransfer
{
    using AppResources;

    public class HintTransferViewModel
    {
        public HintTransferViewModel(string allowTransfer, bool isAlldayTransfer = true)
        {
            AllowTransfer = allowTransfer;
            IsAlldayTransfer = isAlldayTransfer;
        }

        public string AllowTransfer { get; }

        public bool IsAlldayTransfer { get; }
    }
}