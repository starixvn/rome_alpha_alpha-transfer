using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Alpha.Transfer.AppResources;
using Alpha.Transfer.Site.ViewModels.Components;
using Alpha.Transfer.Utils;
using Alpha.Web.Helpers;
using Sunplus.Agent.Model.Entities;
using Alpha.Transfer.Configurations;

namespace Alpha.Transfer.Site.ViewModels.AutoTransfer
{
    public class HistoryViewModel
    {
        private const int reportMonth = -3;

        public HistoryViewModel(IUserSession userSession)
        {
            var minDate = SystemTime.Today.AddDays(-1).AddMonths(reportMonth);
            MinDate = minDate;
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
                new SelectListItem { Text = Resources.First_LastName, Value = "3" },
                new SelectListItem { Text = Resources.TransferAmount, Value = "4" },
                new SelectListItem { Text = Resources.Status, Value = "5" },
                new SelectListItem { Text = Resources.Reason, Value = "6" },
                new SelectListItem { Text = Resources.CompletedTime, Value = "7" },
            };

        public string UserGuideLink => AppSettings.IsMBCSite
            ? MicroSites.HowToUseUrl("~/Index.aspx?linktomessagesite=184099")
            : MicroSites.MessageUrl("~/StaticMessage/Guide?id=184099");
    }
}