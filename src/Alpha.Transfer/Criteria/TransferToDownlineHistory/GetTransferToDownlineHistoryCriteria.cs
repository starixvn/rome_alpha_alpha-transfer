namespace Alpha.Transfer.Criteria.TransferToDownlineHistory
{
    using Fanex.Data.Repository;
    using System;

    public class GetTransferToDownlineHistoryCriteria : CriteriaBase
    {
        public GetTransferToDownlineHistoryCriteria(
            int custId,
            string userName,
            DateTime fromDate,
            DateTime toDate,
            int pageSize,
            int pageIndex)
        {
            CustId = custId;
            UserName = userName;
            FromDate = fromDate;
            ToDate = toDate;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int CustId { get; }

        public string UserName { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        public int TotalRecords { get; set; }

        public override string GetSettingKey() => "Age_Rpt_Downline_Transfer_History";

        public override bool IsValid() => CustId > 0 && FromDate <= ToDate;
    }
}