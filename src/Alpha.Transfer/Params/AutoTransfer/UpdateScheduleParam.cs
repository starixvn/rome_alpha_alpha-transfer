namespace Alpha.Transfer.Params.AutoTransfer
{
    public class UpdateScheduleParam
    {
        public int CustId { get; set; }

        public string CustName { get; set; }

        public string Schedule { get; set; }

        public int UplineId { get; set; }

        public int SubAccId { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public string Old_TransferOption { get; set; }

        public string AGIP { get; set; }
    }
}
