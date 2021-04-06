namespace Alpha.Transfer.Services
{
    using Criteria.TransferToDownlineHistory;
    using DataAccesses;
    using Models.TransferToDownlineHistory;
    using Params.TransferToDownlineHistory;
    using System;
    using System.Collections.Generic;

    public interface TransferToDownlineHistoryService
    {
        Tuple<List<TransferToDownlineHistoryModel>, int> GetTransferToDownlineHistory(TransferToDownlineHistoryPagingParam param);
    }

    public class TransferToDownlineHistoryServiceImpl : TransferToDownlineHistoryService
    {
        private readonly TransferToDownlineHistoryDataAccess dataAccess;

        public TransferToDownlineHistoryServiceImpl(TransferToDownlineHistoryDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public Tuple<List<TransferToDownlineHistoryModel>, int> GetTransferToDownlineHistory(TransferToDownlineHistoryPagingParam param)
            => dataAccess.GetTransferToDownlineHistory(new GetTransferToDownlineHistoryCriteria(
                           param.CustId,
                           param.UserName,
                           param.FromDate,
                           param.ToDate,
                           param.PageSize,
                           param.PageIndex));
    }
}