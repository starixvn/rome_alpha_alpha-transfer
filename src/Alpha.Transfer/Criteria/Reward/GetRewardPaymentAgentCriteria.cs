using System;
using Alpha.Transfer.Models;

namespace Alpha.Transfer.Criteria.Reward
{
    internal class GetRewardPaymentAgentCriteria : GetRewardCriteria
    {
        public GetRewardPaymentAgentCriteria(
            int custId,
            UserLevel custLevel,
            DateTime fromDate,
            DateTime toDate,
            string programIds,
            bool? paymentStatus,
            byte levelType,
            string username) : base(custId, custLevel, fromDate, toDate, programIds, paymentStatus, username)
        {
            LevelType = levelType;
        }

        public byte LevelType { get; }

        public override string GetSettingKey() => "Age_RewardPayment_Agent";
    }
}
