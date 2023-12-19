namespace Alpha.Transfer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Transfer.Criteria.AutoTransfer;
    using Alpha.Transfer.Extensions;
    using Alpha.Transfer.Models.AutoTransfer;
    using Alpha.Transfer.Params.AutoTransfer;
    using Fanex.Data.Repository;
    using Models;

    public interface AutoTransferService
    {
        Task<Result> UpdateAsync(UpdateSingleParam param);
        Task<AutoTransferMonitoringPageModel> GetTodayTransfersAsync(GetTodayTransfersParam param);
        Task<AutoTransferMonitoringPageModel> GetHistoryTransfersAsync(GetHistoryTransfersParam param);
        Task<Result> CancelJobAsync(CancelJobParam param);
        Task<Result> CancelAllJobAsync(CancelAllJobParam param);
        Task<Result> UpdateScheduleAsync(UpdateScheduleParam param);
        Task<UpdateMultipleAutoTransferResultModel> UpdateMultipleAutoTransferAsync(UpdateMultipleAutoTransferParam param);
    }

    public class AutoTransferServiceImpl : AutoTransferService
    {
        private readonly IDynamicRepository repository;

        public AutoTransferServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public async Task<Result> UpdateAsync(UpdateSingleParam param)
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
            var isAutoJobRunning = await GetJobStatus(param.CustId, param.UplineId, "on");
            var criteria = new UpdateScheduleCriteria(param, isAutoJobRunning);

            await repository.ExecuteAsync(criteria);

            var warningCase = string.IsNullOrWhiteSpace(criteria.ErrMsg) ? 0 : -1;

            return new Result
            {
                Code = criteria.ErrCode == 0 ? warningCase : criteria.ErrCode,
                Message = criteria.ErrMsg ?? string.Empty
            };
        }

        public async Task<UpdateMultipleAutoTransferResultModel> UpdateMultipleAutoTransferAsync(UpdateMultipleAutoTransferParam param)
        {
            var offResult = await UpdateMultipleAutoTransferToStatusAsync(param, param.OffStatus, param.OffIds, param.OffNames, param.OffSchedule);

            if (!string.IsNullOrWhiteSpace(offResult.StopMessage))
            {
                return offResult;
            }

            var onResult = await UpdateMultipleAutoTransferToStatusAsync(param, param.OnStatus, param.OnIds, param.OnNames, param.OnSchedule);
            return new UpdateMultipleAutoTransferResultModel
            {
                Code = offResult.Code ?? onResult.Code,
                StopMessage = offResult.StopMessage ?? onResult.StopMessage,
                WarningCusts = offResult.WarningCusts.Concat(onResult.WarningCusts).ToList(),
                ErrorCusts = offResult.ErrorCusts.Concat(onResult.ErrorCusts).ToList(),
            };
        }

        public async Task<UpdateMultipleAutoTransferResultModel> UpdateMultipleAutoTransferToStatusAsync(
            UpdateMultipleAutoTransferParam param,
            string status,
            string pIds,
            string pNames,
            string schedule)
        {
            var stopCodes = new[] { 105, 108 };

            var ids = (pIds ?? string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var names = (pNames ?? string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var dates = (schedule ?? string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var result = new UpdateMultipleAutoTransferResultModel
            {
                WarningCusts = new List<string>(),
                ErrorCusts = new List<string>()
            };

            for (var i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var name = names[i];

                var updateSingleParam = new UpdateSingleParam
                {
                    CustId = int.Parse(id),
                    UplineId = param.UplineId,
                    IP = param.IP,
                    Language = param.Language,
                    State = status,
                    SubAccId = param.SubAccId,
                    Schedule = dates,
                    Old_TransferOption = param.Old_TransferOption
                };

                var singleResult = await UpdateAsync(updateSingleParam);

                if (stopCodes.Any(s => s == singleResult.Code))
                {
                    result.Code = singleResult.Code;
                    result.StopMessage = singleResult.Message;
                    return result;
                }

                if (singleResult.Code != 0)
                {
                    result.ErrorCusts.Add(name);
                    continue;
                }

                if (singleResult.Code == 0 && !string.IsNullOrWhiteSpace(singleResult.Message))
                {
                    result.WarningCusts.Add(name);
                }
            }

            return result;
        }
    }
}
