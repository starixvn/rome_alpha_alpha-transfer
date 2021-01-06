namespace Alpha.Transfer.Site.Handlers
{
    using Alpha.Transfer.Services;
    using MediatR;
    using Requests.TransferToDownlineHistory;
    using System.Linq;
    using ViewModels.TransferToDownlineHistory;

    public class TransferToDownlineHistoryHandler : TransferToDownlineHistoryBaseHandler, IRequestHandler<TransferToDownlineHistoryQuery, TransferToDownlineHistoryViewModel>
    {
        public TransferToDownlineHistoryHandler(TransferToDownlineHistoryService transferToDownlineHistoryService)
            : base(transferToDownlineHistoryService)
        {
        }

        public TransferToDownlineHistoryViewModel Handle(TransferToDownlineHistoryQuery message)
        {
            var param = BuildTransferToDownlineHistoryParam(message);
            var transferToDownlineHistories = GetTransferToDownlineHistory(message);
            var totalRecords = transferToDownlineHistories.Count();
            var records = transferToDownlineHistories
                .Skip((param.Page - 1) * param.PageSize)
                .Take(param.PageSize);

            return new TransferToDownlineHistoryViewModel(records, message, param, totalRecords);
        }
    }
}