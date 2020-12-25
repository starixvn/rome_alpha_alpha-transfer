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
    using System.Web.Mvc;
    using ViewModels.Components;

    public class TransferToDownlineHistoryViewModel
    {
        private const int reportMonth = -2;
        private readonly TransferToDownlineHistoryQuery message;
        private readonly GetTransferToDownlineHistoryParam param;

        public TransferToDownlineHistoryViewModel(
            IEnumerable<TransferToDownlineHistoryModel> transferToDownlineHistories,
            TransferToDownlineHistoryQuery message,
            GetTransferToDownlineHistoryParam param)
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

        public int PageSize => param.PageSize;

        public int Page => param.Page;

        public IEnumerable<SelectListItem> PageSizeOptions => BuildPageSizeOptions();

        public IEnumerable<TransferToDownlineHistoryModel> TransferToDownlineHistories { get; }

        public TransferToDownlineHistoryFilterViewModel FilterViewModel => new TransferToDownlineHistoryFilterViewModel(
            UserName,
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption),
            MinDate,
            PageSize,
            Page);

        private IEnumerable<SelectListItem> BuildPageSizeOptions()
            => new List<SelectListItem>
            {
                new SelectListItem { Text = "10", Value = "10", Selected = PageSize == pageSizeMapper["10"] },
                new SelectListItem { Text = "50", Value = "50", Selected = PageSize == pageSizeMapper["50"] },
                new SelectListItem { Text = "100", Value = "100", Selected = PageSize == pageSizeMapper["100"] },
                new SelectListItem { Text = "500", Value = "500", Selected = PageSize == pageSizeMapper["500"] },
            };

        private readonly IDictionary<string, int> pageSizeMapper = new Dictionary<string, int>
        {
            ["10"] = 10,
            ["50"] = 50,
            ["100"] = 100,
            ["500"] = 500
        };
    }
}