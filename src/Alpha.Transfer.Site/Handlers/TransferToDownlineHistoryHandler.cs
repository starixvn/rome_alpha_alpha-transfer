namespace Alpha.Transfer.Site.Handlers
{
    using Alpha.Transfer.Extensions;
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Alpha.Transfer.Services;
    using MediatR;
    using Requests.TransferToDownlineHistory;
    using ViewModels.TransferToDownlineHistory;

    public class TransferToDownlineHistoryHandler :
        TransferToDownlineHistoryBaseHandler,
        IRequestHandler<TransferToDownlineHistoryQuery, TransferToDownlineHistoryViewModel>,
        IRequestHandler<TransferToDownlineHistoryPagingQuery, TransferToDownlineHistoryPagingViewModel>
    {
        public TransferToDownlineHistoryHandler(TransferToDownlineHistoryService transferToDownlineHistoryService)
            : base(transferToDownlineHistoryService)
        {
        }

        public TransferToDownlineHistoryViewModel Handle(TransferToDownlineHistoryQuery message)
        {
            return new TransferToDownlineHistoryViewModel(message, new TransferToDownlineHistoryParam(
                message.CustId,
                message.UserName,
                message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                message.ToDate));
        }

        public TransferToDownlineHistoryPagingViewModel Handle(TransferToDownlineHistoryPagingQuery message)
        {
            var data = GetTransferToDownlineHistory(new TransferToDownlineHistoryPagingParam(
                message.CustId,
                message.UserName,
                message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                message.ToDate,
                message.PageSize,
                message.PageIndex));

            return new TransferToDownlineHistoryPagingViewModel(data.Item1, data.Item2);
        }
    }
}