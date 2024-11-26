namespace Alpha.Transfer.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public static class IEnumerableExtensions
    {
        public static IEnumerable<SelectListItem> GetSelectListItems(this string[] columns)
            => columns.Select((c, i) => new SelectListItem { Value = (i + 1).ToString(), Text = c });

        public static string ToSeparatedString(this IEnumerable<string> l)
            => string.Join(", ", l);

        public static IEnumerable<int> ToListInt(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var arrayValue = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in arrayValue)
                {
                    if (int.TryParse(item, out var intValue))
                    {
                        yield return intValue;
                    }
                }
            }
        }
    }
}