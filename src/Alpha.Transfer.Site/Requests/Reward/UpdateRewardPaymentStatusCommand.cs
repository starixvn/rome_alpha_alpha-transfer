using Alpha.Transfer.Models;
using Alpha.Transfer.Models.Reward;
using MediatR;

namespace Alpha.Transfer.Site.Requests.Reward
{
    public class UpdateRewardPaymentStatusCommand : TransferBaseQuery, IAsyncRequest<RewardResult>
    {
        public UpdateRewardPaymentStatusCommand(IUser viewer) : base(viewer)
        {
            SubAccId = viewer.SubAccId;
            IP = viewer.IP;
            LanguageKey = viewer.LanguageKey;
            IsInternal = viewer.IsInternal;
        }

        public RewardPaymentStatus[] Data { get; set; }

        public int SubAccId { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public bool IsInternal { get; }

        public override bool IsValid() => !IsInternal && Data != null && Data.Length > 0;
    }
}