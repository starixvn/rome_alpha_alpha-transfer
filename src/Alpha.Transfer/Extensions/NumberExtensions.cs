namespace Alpha.Transfer.Extensions
{
    using System.Globalization;

    public static class NumberExtensions
    {
        private const string commasAlwaysWithDecimalPoint = "#,##0.00";
        private const string commasOptionalWithDecimalPoint = "#,##0.##";

        public static string FormatWithCommasAlwaysWithDecimalPoint(this decimal number)
            => number.ToString(commasAlwaysWithDecimalPoint, CultureInfo.InvariantCulture);

        public static string FormatWithCommasOptionalWithDecimalPoint(this decimal number)
            => number.ToString(commasOptionalWithDecimalPoint, CultureInfo.InvariantCulture);
    }
}