using Alpha.Transfer.Models;
using Alpha.Transfer.Site.ViewModels.Reward;
using MediatR;

namespace Alpha.Transfer.Site.Requests.Reward
{
    public class RewardDetailQuery : RewardBaseQuery, IAsyncRequest<RewardDetailViewModel>
    {
        public RewardDetailQuery(IUser viewer) : base(viewer)
        {
        }
    }
}
