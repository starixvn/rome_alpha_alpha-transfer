namespace Alpha.Transfer.Models.Reward
{
    public class RewardPaymentStatus
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public int ProgramId { get; set; }

        public long MaxTransId { get; set; }
    }
}
