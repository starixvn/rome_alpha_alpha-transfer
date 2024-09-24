using Alpha.Transfer.Models;
using Alpha.Transfer.Site.ViewModels.Reward;
using MediatR;

namespace Alpha.Transfer.Site.Requests.Reward
{
    public class RewardPaymentQuery : RewardBaseQuery, IAsyncRequest<RewardPaymentViewModel>
    {
        public RewardPaymentQuery(IUser viewer) : base(viewer)
        {
        }
    }
}
