using System;
using Alpha.Transfer.Utils;

namespace Alpha.Transfer.Site.ViewModels.Components
{
    public class DateRangePickerJQueryViewModel
    {
        public DateRangePickerJQueryViewModel(
            DateTime fromDate,
            DateTime toDate,
            DateTime minDate,
            DateTime maxDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
            MinDate = minDate;
            MaxDate = maxDate;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime Today { get; } = SystemTime.Today;
    }
}