using System;

namespace Alpha.Transfer.Params.AutoTransfer
{
    public class GetHistoryTransfersParam
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string Status { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int SortedBy { get; set; }

        public int Direction { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
