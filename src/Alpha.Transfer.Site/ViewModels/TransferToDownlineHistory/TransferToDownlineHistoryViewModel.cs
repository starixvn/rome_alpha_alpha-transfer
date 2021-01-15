namespace Alpha.Transfer.Site.ViewModels.TransferToDownlineHistory
{
    using Alpha.Transfer.AppResources;
    using Alpha.Transfer.Models;
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Alpha.Transfer.Utils;
    using Requests.TransferToDownlineHistory;
    using System;
    using System.Collections.Generic;
    using ViewModels.Components;

    public class TransferToDownlineHistoryViewModel
    {
        private const int reportMonth = -2;
        private readonly TransferToDownlineHistoryQuery message;
        private readonly TransferToDownlineHistoryParam param;

        public TransferToDownlineHistoryViewModel(
            IEnumerable<TransferToDownlineHistoryModel> transferToDownlineHistories,
            TransferToDownlineHistoryQuery message,
            TransferToDownlineHistoryParam param)
        {
            this.message = message;
            this.param = param;
            CustLevel = message.CustLevel;
            TransferToDownlineHistories = transferToDownlineHistories;
        }

        public UserLevel CustLevel { get; }

        public string UserName => message.UserName;

        public string DirectDownlineText
            => Resources.ResourceManager.GetString(CustLevel.GetDirectDownLineLevel().DisplayName);

        public static DateTime MinDate => SystemTime.Today.AddMonths(reportMonth);

        public static DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => param.FromDate;

        public DateTime ToDate => param.ToDate;

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public IEnumerable<TransferToDownlineHistoryModel> TransferToDownlineHistories { get; }

        public TransferToDownlineHistoryFilterViewModel FilterViewModel => new TransferToDownlineHistoryFilterViewModel(
            UserName,
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption),
            MinDate);
    }
}