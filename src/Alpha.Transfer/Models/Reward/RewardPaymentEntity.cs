using System;

namespace Alpha.Transfer.Models.Reward
{
    public class RewardPaymentEntity
    {
        public long MaxTransId { get; set; }

        public int ProgramId { get; set; }

        public string ProgramName { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public decimal Amount { get; set; }

        public bool PaymentStatus { get; set; }

        public DateTime? PaymentDate { get; set; }

        public bool IsDirectMember { get; set; }

        public static RewardPaymentEntity operator +(RewardPaymentEntity model1, RewardPaymentEntity model2)
        {
            model1.Amount += model2.Amount;

            return model1;
        }
    }
}
