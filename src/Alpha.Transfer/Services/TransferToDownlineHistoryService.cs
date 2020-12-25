namespace Alpha.Transfer.Services
{
    using Criteria.TransferToDownlineHistory;
    using DataAccesses;
    using Models.TransferToDownlineHistory;
    using Params.TransferToDownlineHistory;
    using System.Collections.Generic;
    using System.Linq;

    public interface TransferToDownlineHistoryService
    {
        IEnumerable<TransferToDownlineHistoryModel> GetTransferToDownlineHistory(GetTransferToDownlineHistoryParam param);
    }

    public class TransferToDownlineHistoryServiceImpl : TransferToDownlineHistoryService
    {
        private readonly TransferToDownlineHistoryDataAccess transferToDownlineHistoryDataAccess;

        public TransferToDownlineHistoryServiceImpl(TransferToDownlineHistoryDataAccess transferToDownlineHistoryDataAccess)
        {
            this.transferToDownlineHistoryDataAccess = transferToDownlineHistoryDataAccess;
        }

        public IEnumerable<TransferToDownlineHistoryModel> GetTransferToDownlineHistory(GetTransferToDownlineHistoryParam param)
            => transferToDownlineHistoryDataAccess.GetTransferToDownlineHistory(new GetTransferToDownlineHistoryCriteria(
                param.CustId,
                param.UserName,
                param.FromDate,
                param.ToDate))
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize);
    }
}