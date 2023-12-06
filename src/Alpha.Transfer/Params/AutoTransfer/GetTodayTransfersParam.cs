namespace Alpha.Transfer.Params.AutoTransfer
{
    public class GetTodayTransfersParam
    {
        public int CustId { get; set; }

        public string Language { get; set; }

        public string Username { get; set; }

        public string Status { get; set; }

        public int SortedBy { get; set; }

        public int Direction { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string AGIP { get; set; }
    }
}
