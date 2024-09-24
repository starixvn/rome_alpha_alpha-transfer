using Alpha.Transfer.Models;
using Alpha.Transfer.Site.ViewModels;
using MediatR;

namespace Alpha.Transfer.Site.Requests.Reward
{
    public class RewardDetailExcelQuery : RewardBaseQuery, IAsyncRequest<ExcelFileViewModel>
    {
        public RewardDetailExcelQuery(IUser viewer) : base(viewer)
        {
            PageIndex = 1;
            PageSize = int.MaxValue - 1;
        }
    }
}