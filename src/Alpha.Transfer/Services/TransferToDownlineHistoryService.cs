namespace Alpha.Transfer.Services
{
    using Criteria.TransferToDownlineHistory;
    using Fanex.Data.Repository;
    using Models.TransferToDownlineHistory;
    using Params.TransferToDownlineHistory;
    using System.Collections.Generic;

    public interface TransferToDownlineHistoryService
    {
        IEnumerable<TransferToDownlineHistoryModel> GetTransferToDownlineHistory(TransferToDownlineHistoryParam param);
    }

    public class TransferToDownlineHistoryServiceImpl : TransferToDownlineHistoryService
    {
        private readonly IDynamicRepository repository;

        public TransferToDownlineHistoryServiceImpl(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<TransferToDownlineHistoryModel> GetTransferToDownlineHistory(TransferToDownlineHistoryParam param)
            => repository.Fetch<TransferToDownlineHistoryModel>(new GetTransferToDownlineHistoryCriteria(
                           param.CustId,
                           param.UserName,
                           param.FromDate,
                           param.ToDate));
    }
}