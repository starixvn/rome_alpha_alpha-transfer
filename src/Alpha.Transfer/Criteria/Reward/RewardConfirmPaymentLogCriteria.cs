using Fanex.Data.Repository;

namespace Alpha.Transfer.Criteria.Reward
{
    internal class RewardConfirmPaymentLogCriteria : NonQueryCommand
    {
        public RewardConfirmPaymentLogCriteria()
        {
        }

        public override string GetSettingKey() => "";

        public override bool IsValid() => true;
    }
}
