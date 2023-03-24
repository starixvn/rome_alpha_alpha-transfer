using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Alpha.Transfer.AppResources;
using Alpha.Transfer.Configurations;
using Alpha.Transfer.Extensions;
using Alpha.Transfer.Site.ViewModels.Components;
using Alpha.Transfer.Utils;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Transfer.Site.ViewModels.AutoTransfer
{
    public class HistoryViewModel
    {
        private const int reportMonth = -3;

        public HistoryViewModel(IUserSession userSession)
        {
            var minDate = SystemTime.Today.AddMonths(reportMonth);
            var supportDate = AppSettings.EnableSupportAutoTransferDate().Value;
            MinDate = minDate < supportDate ? supportDate : minDate;
            DateRangePickerViewModel = new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, 1);
            DateRangePickerViewModel.RangeOptions = DateRangePickerViewModel.RangeOptions.Where(x => x.Value != "0");

            ViewerId = userSession.CustId;
            SubAccId = userSession.SubAccId;
        }

        public DateTime FromDate => SystemTime.Today.AddDays(-1);

        public DateTime ToDate => SystemTime.Today.AddDays(-1);

        public static DateTime MinDate { get; set; }

        public static DateTime MaxDate => SystemTime.Today.AddDays(-1);

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public int ViewerId { get; set; }

        public int SubAccId { get; set; }

        public IEnumerable<SelectListItem> Columns
            => new[] {
                Resources.No,
                Resources.Username,
                Resources.FirstName,
                Resources.TransferAmount,
                Resources.Status,
                Resources.Reason,
                Resources.CompletedTime,
            }.GetSelectListItems();
    }
}