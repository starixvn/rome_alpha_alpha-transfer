using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Alpha.Transfer.AppResources;
using Alpha.Transfer.Site.ViewModels.Components;
using Alpha.Transfer.Utils;
using AlphaPlus.User.Models;

namespace Alpha.Transfer.Site.ViewModels.AutoTransfer
{
    public class HistoryViewModel : AutoTransferViewModel
    {
        private const int reportMonth = -3;

        public HistoryViewModel(IUserSession userSession) : base(userSession)
        {
            MinDate = SystemTime.Today.AddDays(-1).AddMonths(reportMonth);
            DateRangePickerViewModel = new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, 1);
            DateRangePickerViewModel.RangeOptions = DateRangePickerViewModel.RangeOptions.Where(x => x.Value != "0");
            Columns = GetColumns();
        }

        public DateTime FromDate => SystemTime.Today.AddDays(-1);

        public DateTime ToDate => SystemTime.Today.AddDays(-1);

        public static DateTime MinDate { get; private set; }

        public static DateTime MaxDate => SystemTime.Today.AddDays(-1);

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        private IEnumerable<SelectListItem> GetColumns()
        {
            if (IsShowLevelColumn)
            {
                yield return new SelectListItem { Text = Resources.Level, Value = "8" };
            }

            yield return new SelectListItem { Text = Resources.First_LastName, Value = "3" };
            yield return new SelectListItem { Text = Resources.TransferAmount, Value = "4" };
            yield return new SelectListItem { Text = Resources.Status, Value = "5" };
            yield return new SelectListItem { Text = Resources.Reason, Value = "6" };
            yield return new SelectListItem { Text = Resources.CompletedTime, Value = "7" };
        }
    }
}