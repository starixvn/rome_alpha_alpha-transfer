namespace Alpha.Transfer.Params.AutoTransfer
{
    public class UpdateStatusParam
    {
        public int CustId { get; set; }

        public int UplineId { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public string State { get; set; }

        public string[] Schedule { get; set; }

        public int SubAccId { get; set; }
    }
}
