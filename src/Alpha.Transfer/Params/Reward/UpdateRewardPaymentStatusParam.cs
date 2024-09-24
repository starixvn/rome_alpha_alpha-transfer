using Alpha.Transfer.Models.Reward;

namespace Alpha.Transfer.Params.Reward
{
    public class UpdateRewardPaymentStatusParam
    {
        public UpdateRewardPaymentStatusParam(
            RewardPaymentStatus[] data,
            int actorId,
            int subAccId,
            string ip,
            string language)
        {
            Data = data;
            ActorId = actorId;
            SubAccId = subAccId;
            IP = ip;
            Language = language;
        }

        public RewardPaymentStatus[] Data { get; }

        public int ActorId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string Language { get; }
    }
}
