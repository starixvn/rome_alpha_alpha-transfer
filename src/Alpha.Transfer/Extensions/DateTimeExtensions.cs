namespace Alpha.Transfer.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        private const string ShortDateWithSlash = "MM/dd/yyyy";
        private const string LongDateWithSlash = "MM/dd/yyyy hh:mm:ss tt";
        private const string LongDateWithSlash24HFormat = "MM/dd/yyyy HH:mm:ss";
        private const string LongDate24HFormat = "yyyyMMdd HH:mm:ss";

        public static string ToShortDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(ShortDateWithSlash);

        public static string ToLongDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(LongDateWithSlash);

        public static string ToLongDateWithSlash24HFormat(this DateTime dateTime)
            => dateTime.ToString(LongDateWithSlash24HFormat);

        public static string ToLongDate24HFormat(this DateTime dateTime)
            => dateTime.ToString(LongDate24HFormat);

        public static DateTime GetGreaterDate(this DateTime fromDate, DateTime limitedDate)
            => fromDate < limitedDate ? limitedDate : fromDate;

        public static DateTime FirstDayOfMonth(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, 1);
    }
}