namespace Alpha.Transfer.Criteria.Reward
{
    using Fanex.Data.Repository;

    internal class UpdateRewardPaymentStatusCriteria : NonQueryCommand
    {
        public UpdateRewardPaymentStatusCriteria(
            string data,
            int creator,
            int subAccId,
            string subAccName,
            string ip,
            string language)
        {
            Data = data;
            Creator = creator;
            SubAccId = subAccId;
            SubAccName = subAccName;
            IP = ip;
            Language = language;
        }

        public string Data { get; }

        public int Creator { get; }

        public int SubAccId { get; }

        public string SubAccName { get; }

        public string IP { get; }

        public string Language { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string PaymentLog { get; set; }

        public override string GetSettingKey() => "SiteDB_Acc_RewardPayment_Confirm";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(Data) && Creator > 0;
    }
}
