namespace Alpha.Transfer.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public static class IEnumerableExtensions
    {
        public static IEnumerable<SelectListItem> GetSelectListItems(this string[] columns)
            => columns.Select((c, i) => new SelectListItem { Value = (i + 1).ToString(), Text = c });
    }
}