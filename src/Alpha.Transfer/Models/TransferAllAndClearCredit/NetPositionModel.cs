namespace Alpha.Transfer.Models.TransferAllAndClearCredit
{
    public class NetPositionModel
    {
        public int CustId { get; }

        public decimal NetPosition { get; }

        public string ToXML
            => $"<r custId=\"{CustId}\" netPosition=\"{NetPosition}\" />";
    }
}
