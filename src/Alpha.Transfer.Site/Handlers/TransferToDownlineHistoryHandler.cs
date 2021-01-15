namespace Alpha.Transfer.Site.Handlers
{
    using Alpha.Transfer.Services;
    using MediatR;
    using Requests.TransferToDownlineHistory;
    using ViewModels.TransferToDownlineHistory;

    public class TransferToDownlineHistoryHandler : TransferToDownlineHistoryBaseHandler, IRequestHandler<TransferToDownlineHistoryQuery, TransferToDownlineHistoryViewModel>
    {
        public TransferToDownlineHistoryHandler(TransferToDownlineHistoryService transferToDownlineHistoryService)
            : base(transferToDownlineHistoryService)
        {
        }

        public TransferToDownlineHistoryViewModel Handle(TransferToDownlineHistoryQuery message)
            => new TransferToDownlineHistoryViewModel(
                GetTransferToDownlineHistory(message),
                message,
                BuildTransferToDownlineHistoryParam(message));
    }
}