namespace Alpha.Transfer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Transfer.Criteria.AutoTransfer;
    using Alpha.Transfer.Extensions;
    using Alpha.Transfer.Helpers;
    using Alpha.Transfer.Models.AutoTransfer;
    using Alpha.Transfer.Params.AutoTransfer;
    using Fanex.Data.Repository;
    using Models;

    public interface AutoTransferService
    {
        Task<Result> UpdateAsync(UpdateStatusParam param);
        Task<AutoTransferMonitoringPageModel> GetTodayTransfersAsync(GetTodayTransfersParam param);
        Task<AutoTransferMonitoringPageModel> GetHistoryTransfersAsync(GetHistoryTransfersParam param);
        Task<Result> CancelJobAsync(CancelJobParam param);
        Task<Result> CancelAllJobAsync(CancelAllJobParam param);
        Task<Result> UpdateScheduleAsync(UpdateScheduleParam param);
        Task<Result> UpdateMultipleAutoTransferAsync(UpdateMultipleAutoTransferParam param);
    }

    public class AutoTransferServiceImpl : AutoTransferService
    {
        private readonly IDynamicRepository repository;

        public AutoTransferServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public async Task<Result> UpdateAsync(UpdateStatusParam param)
        {
            var isAutoJobRunning = await GetJobStatus(param.CustId, param.UplineId, param.State);
            var criteria = new UpdateStatusCriteria(param, isAutoJobRunning);

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
                Direction = param.Direction,


                TotalCancelAll = criteria.TotalCancelAll,
                TotalPending = criteria.TotalPending,
                TotalProcessing = criteria.TotalProcessing,
                TotalFailed = criteria.TotalFailed,
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

        public async Task<Result> CancelAllJobAsync(CancelAllJobParam param)
        {
            var criteria = new CancelAllJobCriteria(param);

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

        public async Task<Result> UpdateScheduleAsync(UpdateScheduleParam param)
        {
            var criteria = new UpdateScheduleCriteria(param);

            await repository.ExecuteAsync(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? string.Empty : criteria.ErrMsg
            };
        }

        public async Task<Result> UpdateMultipleAutoTransferAsync(UpdateMultipleAutoTransferParam param)
        {
            var messages = new List<string>();

            var onIds = (param.OnIds ?? string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var id in onIds)
            {
                var updateSingleParam = new UpdateStatusParam
                {
                    CustId = int.Parse(id),
                    UplineId = param.UplineId,
                    IP = param.IP,
                    Language = param.Language,
                    State = param.OnStatus,
                    SubAccId = param.SubAccId,
                    Schedule = (param.OnSchedule ?? string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                };

                var isAutoJobRunning = await GetJobStatus(updateSingleParam.CustId, param.UplineId, param.OnStatus);
                var criteria = new UpdateStatusCriteria(updateSingleParam, isAutoJobRunning);
                await repository.ExecuteAsync(criteria);

                if (criteria.ErrCode != 0)
                {
                    messages.Add(criteria.ErrMsg);
                }
            }

            var offIds = (param.OffIds ?? string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var id in offIds)
            {
                var updateSingleParam = new UpdateStatusParam
                {
                    CustId = int.Parse(id),
                    UplineId = param.UplineId,
                    IP = param.IP,
                    Language = param.Language,
                    State = param.OffStatus,
                    SubAccId = param.SubAccId,
                    Schedule = (param.OffSchedule ?? string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                };

                var isAutoJobRunning = await GetJobStatus(updateSingleParam.CustId, param.UplineId, param.OffStatus);
                var criteria = new UpdateStatusCriteria(updateSingleParam, isAutoJobRunning);
                await repository.ExecuteAsync(criteria);

                if (criteria.ErrCode != 0)
                {
                    messages.Add(criteria.ErrMsg);
                }
            }

            return new Result
            {
                Code = messages.Any() ? 1 : 0,
                Message = string.Join("<br />", messages)
            };
        }
    }
}
