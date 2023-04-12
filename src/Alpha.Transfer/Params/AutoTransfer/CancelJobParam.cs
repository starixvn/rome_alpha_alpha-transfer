namespace Alpha.Transfer.Params.AutoTransfer
{
    public class CancelJobParam
    {
        public int FromCustId { get; set; }

        public int ToCustId { get; set; }

        public string ViewStatus { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }
    }
}
