using System;
using Fanex.Data.Repository;

namespace Alpha.Transfer.Criteria.AutoTransfer
{
    public class GetHistoryTransfersCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string Status { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalItems { get; set; }

        public int SortedBy { get; set; }

        public int Direction { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => "Age_ClearBalanceCust_MonitorSel";
    }
}
