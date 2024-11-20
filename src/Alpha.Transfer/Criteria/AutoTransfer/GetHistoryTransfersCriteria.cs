using System;
using Fanex.Data.Repository;

namespace Alpha.Transfer.Criteria.AutoTransfer
{
    public class GetHistoryTransfersCriteria : CriteriaBase
    {
        public int FromCustId { get; set; }

        public string Language { get; set; }

        public string Searchname { get; set; }

        public string ViewStatus { get; set; }

        public byte LevelType { get; set; }

        public int SortBy { get; set; }

        public int SortType { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRow { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override bool IsValid() => !string.IsNullOrWhiteSpace(ViewStatus);

        public override string GetSettingKey() => "Age_ClearBalanceCust_ProcessHistorySel";
    }
}
