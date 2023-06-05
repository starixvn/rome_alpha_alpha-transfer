namespace Alpha.Transfer.Models.TransferAllAndClearCredit
{
    public class TotalAmtModel
    {
        public int CustId { get; }

        public decimal Amount { get; }

        public string ToXML
            => $"<R ID=\"{CustId}\" AM=\"{Amount}\" />";
    }
}
