namespace Alpha.Transfer.Criteria.Reward
{
    using Fanex.Data.Repository;

    internal class UpdateRewardPaymentStatusCriteria : NonQueryCommand
    {
        public UpdateRewardPaymentStatusCriteria(
            string data,
            int creator,
            int subAccId,
            string ip,
            string language)
        {
            Data = data;
            Creator = creator;
            SubAccId = subAccId;
            IP = ip;
            Language = language;
        }

        public string Data { get; }

        public int Creator { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string Language { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string PaymentLog { get; set; }

        public override string GetSettingKey() => "Acc_RewardPayment_Confirm";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(Data) && Creator > 0;
    }
}
