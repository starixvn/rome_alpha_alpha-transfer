namespace Alpha.Transfer.Site.ViewModels.TransferToDownlineHistory
{
    using Alpha.Transfer.AppResources;
    using Alpha.Transfer.Extensions;
    using System;
    using ViewModels.Components;

    public class TransferToDownlineHistoryFilterViewModel
    {
        public TransferToDownlineHistoryFilterViewModel(
            string userName,
            DateRangePickerViewModel dateRangePickerViewModel,
            DateTime minDate)
        {
            UserName = userName;
            DateRangePickerViewModel = dateRangePickerViewModel;
            ViewDataInstructionMessage = string.Format(
                Resources.CanViewReportDataFrom,
                minDate.ToShortDateWithSlash());
        }

        public string UserName { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public string ViewDataInstructionMessage { get; }
    }
}