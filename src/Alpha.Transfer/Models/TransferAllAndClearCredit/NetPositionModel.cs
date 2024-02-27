namespace Alpha.Transfer.Models.TransferAllAndClearCredit
{
    public class NetPositionModel
    {
        public int CustId { get; set; }

        public decimal NetPosition { get; set; }

        public string ToXML
            => $"<r custId=\"{CustId}\" netPosition=\"{NetPosition}\" />";
    }
}
