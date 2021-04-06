namespace Alpha.Transfer.Site.Requests.TransferToDownlineHistory
{
    using Alpha.Transfer.Models;
    using MediatR;
    using ViewModels.TransferToDownlineHistory;

    public class TransferToDownlineHistoryQuery : TransferToDownlineHistoryBaseQuery, IRequest<TransferToDownlineHistoryViewModel>
    {
        public TransferToDownlineHistoryQuery(IUser viewer) : base(viewer)
        {
        }
    }
}