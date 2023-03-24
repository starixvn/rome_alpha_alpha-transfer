namespace Alpha.Transfer.Site.ViewModels.CashBalanceTransfer
{
    using AppResources;

    public class HintTransferViewModel
    {
        public HintTransferViewModel(
            string allowTransfer,
            bool showWarningTransfer,
            string conditions,
            bool isAlldayTransfer = true)
        {
            AllowTransfer = allowTransfer;
            IsAlldayTransfer = isAlldayTransfer;
            Conditions = conditions;
        }

        public string AllowTransfer { get; }

        public bool IsAlldayTransfer { get; }

        public string Conditions { get; }
    }
}