namespace Alpha.Transfer.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        private const string ShortDateWithSlash = "MM/dd/yyyy";
        private const string LongDateWithSlash = "MM/dd/yyyy hh:mm:ss tt";
        private const string LongDateWithSlash24HFormat = "MM/dd/yyyy HH:mm:ss";

        public static string ToShortDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(ShortDateWithSlash);

        public static string ToLongDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(LongDateWithSlash);

        public static string ToLongDateWithSlash24HFormat(this DateTime dateTime)
            => dateTime.ToString(LongDateWithSlash24HFormat);
    }
}