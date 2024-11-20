using System;
using Alpha.Transfer.Models;

namespace Alpha.Transfer.Criteria.Reward
{
    internal class GetRewardPaymentMemberCriteria : GetRewardCriteria
    {
        public GetRewardPaymentMemberCriteria(
            int custId,
            UserLevel custLevel,
            DateTime fromDate,
            DateTime toDate,
            string programIds,
            bool? paymentStatus,
            string username) : base(custId, custLevel, fromDate, toDate, programIds, paymentStatus, username)
        {
        }

        public override string GetSettingKey() => "Age_RewardPayment_Member";
    }
}
