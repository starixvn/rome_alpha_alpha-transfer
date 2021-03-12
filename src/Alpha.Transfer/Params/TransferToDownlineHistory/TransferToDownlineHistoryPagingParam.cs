namespace Alpha.Transfer.Params.TransferToDownlineHistory
{
    using Constants;
    using System;

    public class TransferToDownlineHistoryPagingParam : TransferToDownlineHistoryParam
    {
        public TransferToDownlineHistoryPagingParam(
            int custId,
            string userName,
            DateTime fromDate,
            DateTime toDate,
            int pageSize,
            int pageIndex) : base(custId, userName, fromDate, toDate)
        {
            PageSize = pageSize < 0 ? PagerConstant.DefaultPageSize : pageSize;
            PageIndex = pageIndex < 0 ? PagerConstant.DefaultPageIndex : pageIndex;
        }

        public int PageSize { get; }

        public int PageIndex { get; }
    }
}