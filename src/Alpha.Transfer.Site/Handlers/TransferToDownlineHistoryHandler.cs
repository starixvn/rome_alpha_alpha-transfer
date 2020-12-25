namespace Alpha.Transfer.Site.Handlers
{
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Alpha.Transfer.Services;
    using MediatR;
    using Requests.TransferToDownlineHistory;
    using ViewModels.TransferToDownlineHistory;

    public class TransferToDownlineHistoryHandler : IRequestHandler<TransferToDownlineHistoryQuery, TransferToDownlineHistoryViewModel>
    {
        private readonly TransferToDownlineHistoryService transferToDownlineHistoryService;

        public TransferToDownlineHistoryHandler(TransferToDownlineHistoryService transferToDownlineHistoryService)
        {
            this.transferToDownlineHistoryService = transferToDownlineHistoryService;
        }

        public TransferToDownlineHistoryViewModel Handle(TransferToDownlineHistoryQuery message)
        {
            var param = new GetTransferToDownlineHistoryParam(message.CustId, message.UserName, message.FromDate, message.ToDate, message.PageSize, message.Page);
            var transferToDownlineHistories = transferToDownlineHistoryService.GetTransferToDownlineHistory(param);

            return new TransferToDownlineHistoryViewModel(transferToDownlineHistories, message, param);
        }
    }
}