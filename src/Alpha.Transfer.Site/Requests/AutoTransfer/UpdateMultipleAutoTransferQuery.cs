namespace Alpha.Transfer.Site.Requests.AutoTransfer
{
    using System.Collections.Generic;
    using Alpha.Transfer.Helpers;
    using Transfer.Models;

    public class UpdateMultipleAutoTransferQuery
    {
        public UpdateMultipleAutoTransferQuery(IUser user)
        {
        }

        public string UplineCondition { get; set; }

        public string OnIds { get; set; }

        public string OnNames { get; set; }

        public string OffIds { get; set; }

        public string OffNames { get; set; }

        public bool IsMixStatus => !string.IsNullOrWhiteSpace(OnIds) && !string.IsNullOrWhiteSpace(OffIds);
        public List<int> TransferrableDates => TransferConditionHelper.GetTransferrableDatesFromDates(UplineCondition);
        public string AllNames => (OnNames + "," + OffNames).Trim(new[] { ',' }).Replace(",", ", ");
    }
}