namespace Alpha.Transfer.Site.Requests.TransferToDownlineHistory
{
    using Alpha.Transfer.Models;
    using MediatR;
    using ViewModels.TransferToDownlineHistory;

    public class TransferToDownlineHistoryPagingQuery : TransferToDownlineHistoryBaseQuery, IRequest<TransferToDownlineHistoryPagingViewModel>
    {
        public TransferToDownlineHistoryPagingQuery(IUser viewer) : base(viewer)
        {
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}