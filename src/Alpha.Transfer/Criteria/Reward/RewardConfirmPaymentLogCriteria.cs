using Fanex.Data.Repository;

namespace Alpha.Transfer.Criteria.Reward
{
    internal class RewardConfirmPaymentLogCriteria : NonQueryCommand
    {
        public RewardConfirmPaymentLogCriteria(string paymentLog)
            => PaymentLog = paymentLog;

        public string PaymentLog { get; }

        public override string GetSettingKey() => "WriteLog_UserLog_RunSP_RewardPayment";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(PaymentLog);
    }
}
