namespace Alpha.Transfer.Services
{
    using System.Threading.Tasks;
    using Alpha.Transfer.Criteria.AutoTransfer;
    using Alpha.Transfer.Extensions;
    using Alpha.Transfer.Models.AutoTransfer;
    using Alpha.Transfer.Params.AutoTransfer;
    using Fanex.Data.Repository;
    using Models;

    public interface AutoTransferService
    {
        Task<Result> UpdateAsync(int custId, int creator, string ip, string language, string state);
        Task<AutoTransferMonitoringPageModel> GetTodayTransfersAsync(GetTodayTransfersParam param);
        Task<AutoTransferMonitoringPageModel> GetHistoryTransfersAsync(GetHistoryTransfersParam param);
        Task<Result> CancelJobAsync(CancelJobParam param);
    }

    public class AutoTransferServiceImpl : AutoTransferService
    {
        private readonly IDynamicRepository repository;

        public AutoTransferServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public async Task<Result> UpdateAsync(int custId, int creator, string ip, string language, string state)
        {
            var isAutoJobRunning = await GetJobStatus(custId, creator, state);
            var criteria = new UpdateMultipleCriteria(creator, custId, isAutoJobRunning, language, ip, state);

            await repository.ExecuteAsync(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };
        }

        public async Task<AutoTransferMonitoringPageModel> GetTodayTransfersAsync(GetTodayTransfersParam param)
        {
            var criteria = new GetTodayTransfersCriteria
            {
                FromCustId = param.CustId,
                Language = param.Language,
                Searchname = param.Username,
                ViewStatus = param.Status,
                SortBy = param.SortedBy,
                SortType = param.Direction,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex
            };

            var transfers = await repository.FetchAsync<AutoTransferMonitoringModel>(criteria);

            return new AutoTransferMonitoringPageModel
            {
                Transfers = transfers,
                TotalItems = criteria.TotalRow,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                SortedBy = param.SortedBy,
                Direction = param.Direction
            };
        }

        public async Task<AutoTransferMonitoringPageModel> GetHistoryTransfersAsync(GetHistoryTransfersParam param)
        {
            var criteria = new GetHistoryTransfersCriteria
            {
                FromCustId = param.CustId,
                Language = param.Language,
                Searchname = param.Username,
                ViewStatus = param.Status,
                SortBy = param.SortedBy,
                SortType = param.Direction,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                FromDate = param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                ToDate = param.ToDate.GetGreaterDate(param.LimitedDateToShowInfo),
            };

            var transfers = await repository.FetchAsync<AutoTransferMonitoringModel>(criteria);


            return new AutoTransferMonitoringPageModel
            {
                Transfers = transfers,
                TotalItems = criteria.TotalRow,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                SortedBy = param.SortedBy,
                Direction = param.Direction
            };
        }

        public async Task<Result> CancelJobAsync(CancelJobParam param)
        {
            var criteria = new CancelJobCriteria(param);

            await repository.ExecuteAsync(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? string.Empty : criteria.ErrMsg
            };
        }

        private async Task<bool> GetJobStatus(int custId, int creator, string state)
        {
            var criteria = new GetJobStatusCriteria(creator, custId, state);

            await repository.ExecuteAsync(criteria);

            return criteria.IsAutoJobRunning;
        }
    }
}
