namespace Alpha.Transfer.Site.ViewModels.CashBalanceTransfer
{
    using AppResources;

    public class HintTransferViewModel
    {
        public HintTransferViewModel(string allowTransfer, bool showWarningTransfer)
        {
            AllowTransfer = allowTransfer;

            if (showWarningTransfer)
            {
                WarningTransfer = Resources.WarningMultipleTransfer;
            }
        }

        public string AllowTransfer { get; }

        public string WarningTransfer { get; }
    }
}