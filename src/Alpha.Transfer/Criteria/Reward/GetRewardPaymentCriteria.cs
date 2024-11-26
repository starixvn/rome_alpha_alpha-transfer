using System;
using Alpha.Transfer.Models;

namespace Alpha.Transfer.Criteria.Reward
{
    internal class GetRewardPaymentCriteria : GetRewardCriteria
    {
        public GetRewardPaymentCriteria(
            int custId,
            UserLevel custLevel,
            DateTime fromDate,
            DateTime toDate,
            string programIds,
            bool? paymentStatus,
            string username) : base(custId, custLevel, fromDate, toDate, programIds, paymentStatus, username)
        {
        }

        public override string GetSettingKey()
            => CustLevel.IsSuperLevel()
            ? "Age_RewardPayment_Master"
            : CustLevel.IsMasterLevel()
                ? "Age_RewardPayment_Agent"
                : "Age_RewardPayment_Member";
    }
}
