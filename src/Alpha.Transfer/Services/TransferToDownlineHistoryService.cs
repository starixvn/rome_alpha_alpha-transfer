namespace Alpha.Transfer.Services
{
    using System;
    using System.Collections.Generic;
    using Criteria.TransferToDownlineHistory;
    using Fanex.Data.Repository;
    using Models.TransferToDownlineHistory;
    using Params.TransferToDownlineHistory;

    public interface TransferToDownlineHistoryService
    {
        Tuple<IEnumerable<TransferToDownlineHistoryModel>, int> GetTransferToDownlineHistory(TransferToDownlineHistoryPagingParam param);
    }

    public class TransferToDownlineHistoryServiceImpl : TransferToDownlineHistoryService
    {
        private readonly IDynamicRepository repository;

        public TransferToDownlineHistoryServiceImpl(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public Tuple<IEnumerable<TransferToDownlineHistoryModel>, int> GetTransferToDownlineHistory(TransferToDownlineHistoryPagingParam param)
        {
            var criteria = new GetTransferToDownlineHistoryCriteria(
                param.CustId,
                param.UserName,
                param.FromDate,
                param.ToDate,
                param.LevelType,
                param.PageSize,
                param.PageIndex);
            var transferToDownlines = repository.Fetch<TransferToDownlineHistoryModel>(criteria);

            return Tuple.Create(transferToDownlines, criteria.TotalRecords);
        }
    }
}