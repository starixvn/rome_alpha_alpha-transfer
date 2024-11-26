using System.Collections.Generic;
using System.Linq;

namespace Alpha.Transfer.Models.Reward
{
    public class RewardPayment
    {
        public RewardPayment(IEnumerable<RewardPaymentEntity> rewardPayments, RewardPaymentEntity total)
        {
            RewardPayments = rewardPayments;
            Total = total;
            var totalItems = rewardPayments.Count();
            Any = totalItems > 0;
            MoreThanOne = totalItems > 1;
        }

        public IEnumerable<RewardPaymentEntity> RewardPayments { get; }

        public RewardPaymentEntity Total { get; }

        public bool Any { get; }

        public bool MoreThanOne { get; }
    }
}
