namespace Alpha.Transfer.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Alpha.Transfer.Services;

    public class TransferToDownlineHistoryBaseHandler : ExcelBaseHandler
    {
        private readonly TransferToDownlineHistoryService transferToDownlineHistoryService;

        public TransferToDownlineHistoryBaseHandler(TransferToDownlineHistoryService transferToDownlineHistoryService)
        {
            this.transferToDownlineHistoryService = transferToDownlineHistoryService;
        }

        protected internal Tuple<IEnumerable<TransferToDownlineHistoryModel>, int> GetTransferToDownlineHistory(TransferToDownlineHistoryPagingParam param)
            => transferToDownlineHistoryService.GetTransferToDownlineHistory(param);
    }
}