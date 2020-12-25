namespace Alpha.Transfer.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        private const string ShortDateWithSlash = "MM/dd/yyyy";
        private const string LongDateWithSlash = "MM/dd/yyyy hh:mm:ss tt";

        private const string LongTime = "hh:mm:ss tt";
        private const string FullDateName = "dddd";
        private const int MonthsInYear = 12;

        public static string ToShortDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(ShortDateWithSlash);

        public static string ToLongDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(LongDateWithSlash);

        public static string ToLongTime(this DateTime dateTime)
            => dateTime.ToString(LongTime);

        public static string ToFullDateName(this DateTime dateTime)
            => dateTime.ToString(FullDateName);

        public static DateTime FirstDayOfMonth(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, 1);

        public static DateTime LastDayOfMonth(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.DaysInMonth());

        public static DateTime FirstDayOfYear(this DateTime dateTime)
            => new DateTime(dateTime.Year, 1, 1);

        public static bool IsSameMonthWith(this DateTime dateTime, DateTime otherDateTime)
            => dateTime.Month == otherDateTime.Month && dateTime.Year == otherDateTime.Year;

        public static bool IsEndOfMonth(this DateTime dateTime)
            => dateTime.Day == DaysInMonth(dateTime);

        public static int GetMonthDiff(this DateTime dateTime, DateTime otherDateTime)
            => (dateTime.Month - otherDateTime.Month + (MonthsInYear * (dateTime.Year - otherDateTime.Year)));

        private static int DaysInMonth(this DateTime dateTime)
            => DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

        public static DateTime GetGreaterDate(this DateTime fromDate, DateTime limitedDate)
            => fromDate < limitedDate ? limitedDate : fromDate;
    }
}