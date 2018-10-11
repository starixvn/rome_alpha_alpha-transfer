namespace Alpha.Transfer.Models.MultipleTransfer
{
    public class CustomerInformation
    {
        public string Username { get; set; }

        public int CustId { get; set; }

        public decimal YesterdayBalance { get; set; }

        public decimal Balance { get; set; }

        public decimal YesterdayCashBalance { get; set; }

        public decimal CashBalance { get; set; }

        public decimal Outstanding { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal UsedCredit { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool Enabled { get; set; }

        public bool Closed { get; set; }

        public bool Suspended { get; set; }

        public string ClosedReason { get; set; }
    }
}
