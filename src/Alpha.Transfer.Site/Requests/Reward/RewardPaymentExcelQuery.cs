using Alpha.Transfer.Models;
using Alpha.Transfer.Site.ViewModels;
using MediatR;

namespace Alpha.Transfer.Site.Requests.Reward
{
    public class RewardPaymentExcelQuery : RewardBaseQuery, IAsyncRequest<ExcelFileViewModel>
    {
        public RewardPaymentExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}