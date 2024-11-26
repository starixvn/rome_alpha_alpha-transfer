using Alpha.Transfer.Models;
using Alpha.Transfer.Models.Reward;
using MediatR;

namespace Alpha.Transfer.Site.Requests.Reward
{
    public class GetRewardPaymentDataQuery : RewardBaseQuery, IAsyncRequest<RewardPayment>
    {
        public GetRewardPaymentDataQuery(IUser viewer) : base(viewer)
        {
            IsInternal = viewer.IsInternal;
        }

        public bool IsInternal { get; }
    }
}