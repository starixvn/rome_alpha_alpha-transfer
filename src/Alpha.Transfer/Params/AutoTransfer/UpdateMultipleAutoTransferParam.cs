namespace Alpha.Transfer.Params.AutoTransfer
{
    public class UpdateMultipleAutoTransferParam
    {
        public int UplineId { get; set; }
        public int SubAccId { get; set; }
        public string IP { get; set; }
        public string Language { get; set; }

        public string OnIds { get; set; }
        public string OffIds { get; set; }
        public string OnStatus { get; set; }
        public string OffStatus { get; set; }
        public string OnSchedule { get; set; }
        public string OffSchedule { get; set; }
    }
}
