namespace Alpha.Transfer.Site.ViewModels.TransferToDownlineHistory
{
    using Alpha.Transfer.AppResources;
    using Alpha.Transfer.Extensions;
    using Alpha.Transfer.Models;
    using System;
    using ViewModels.Components;

    public class TransferToDownlineHistoryFilterViewModel
    {
        public TransferToDownlineHistoryFilterViewModel(
            string userName,
            DateRangePickerViewModel dateRangePickerViewModel,
            DateTime minDate,
            UserLevel actorLevel,
            byte levelType)
        {
            UserName = userName;
            DateRangePickerViewModel = dateRangePickerViewModel;
            ViewDataInstructionMessage = string.Format(
                Resources.CanViewReportDataFrom,
                minDate.ToShortDateWithSlash());
            ActorLevel = actorLevel;
            LevelType = levelType;
        }

        public string UserName { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public string ViewDataInstructionMessage { get; }

        public UserLevel ActorLevel { get; }

        public byte LevelType { get; }
    }
}