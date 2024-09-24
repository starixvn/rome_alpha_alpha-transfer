using Alpha.Transfer.Models;
using Alpha.Transfer.Site.ViewModels.Reward;
using MediatR;

namespace Alpha.Transfer.Site.Requests.Reward
{
    public class GetRewardDetailDataQuery : RewardBaseQuery, IAsyncRequest<RewardDetailDataViewModel>
    {
        public GetRewardDetailDataQuery(IUser viewer) : base(viewer)
        {
        }
    }
}