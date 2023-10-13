using System;
using System.Collections.Generic;
using System.Linq;
using Alpha.Transfer.AppResources;

namespace Alpha.Transfer.Helpers
{
    public static class TransferConditionHelper
    {
        public static List<int> GetTransferrableDatesFromSchedule(string schedule)
        {
            var sc = schedule ?? String.Empty;

            var ds = sc.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s))
                .ToList();

            return ds;
        }

        public static string GetAutoTransferScheduleFromDates(string selectedDates)
            => selectedDates ?? string.Empty;

        public static string GetAutoTransferScheduleFromDates(string[] selectedDates)
            => string.Join(",", selectedDates);

        public static List<int> GetTransferrableDatesFromDates(string selectedDates)
            => (selectedDates ?? String.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => int.Parse(s))
            .ToList();

        public static string GetDateName(int dateIndex)
        {
            string[] dates = { Resources.Sun, Resources.Mon, Resources.Tue, Resources.Wed, Resources.Thurs, Resources.Fri, Resources.Sat };

            return dates[dateIndex - 1];
        }
    }
}
