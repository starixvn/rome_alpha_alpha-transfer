namespace Alpha.Transfer.Site.ViewModels.CashBalanceTransfer
{
    using AppResources;

    public class HintTransferViewModel
    {
        public HintTransferViewModel(string allowTransfer, bool showWarningTransfer, bool isAlldayTransfer = true)
        {
            AllowTransfer = allowTransfer;
            IsAlldayTransfer = isAlldayTransfer;

            if (showWarningTransfer)
            {
                WarningTransfer = Resources.WarningMultipleTransfer;
            }
        }

        public string AllowTransfer { get; }

        public string WarningTransfer { get; }

        public bool IsAlldayTransfer { get; }
    }
}