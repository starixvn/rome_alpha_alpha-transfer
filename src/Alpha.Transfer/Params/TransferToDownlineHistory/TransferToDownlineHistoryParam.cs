namespace Alpha.Transfer.Params.TransferToDownlineHistory
{
    using Constants;
    using System;
    using Utils;

    public class TransferToDownlineHistoryParam
    {
        public TransferToDownlineHistoryParam(
            int custId,
            string userName,
            DateTime fromDate,
            DateTime toDate,
            int pageSize,
            int page)
        {
            CustId = custId;
            UserName = userName;
            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today : fromDate;
            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;
            PageSize = pageSize <= 0 ? PagerConstant.DefaultPageSize : pageSize;
            Page = page <= 0 ? PagerConstant.DefaultPageIndex : page;
        }

        public int CustId { get; }

        public string UserName { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int PageSize { get; }

        public int Page { get; }
    }
}