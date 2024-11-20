using System;
using Alpha.Transfer.Models.Reward;

namespace Alpha.Transfer.Params.Reward
{
    public class UpdateRewardPaymentStatusParam
    {
        public UpdateRewardPaymentStatusParam(
            DateTime fromDate,
            DateTime toDate,
            RewardPaymentStatus[] data,
            int actorId,
            int subAccId,
            string subAccName,
            string ip,
            string language,
            string agIP)
        {
            FromDate = fromDate;
            ToDate = toDate;
            Data = data;
            ActorId = actorId;
            SubAccId = subAccId;
            SubAccName = subAccName;
            IP = ip;
            Language = language;
            AGIP = agIP;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public RewardPaymentStatus[] Data { get; }

        public int ActorId { get; }

        public int SubAccId { get; }

        public string SubAccName { get; }

        public string IP { get; }

        public string Language { get; }

        public string AGIP { get; }
    }
}
