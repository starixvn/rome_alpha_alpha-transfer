namespace Alpha.Transfer.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Transfer.Criteria.AutoTransfer;
    using Alpha.Transfer.Models.AutoTransfer;
    using Alpha.Transfer.Params.AutoTransfer;
    using Criteria.CashBalanceTransfer;
    using Fanex.Data.Repository;
    using Models;

    public interface AutoTransferService
    {
        Task<Result> UpdateAsync(int custId, int creator, string ip, string language, string state);
        Task<AutoTransferMonitoringPageModel> GetTodayTransfersAsync(GetTodayTransfersParam param);
        Task<AutoTransferMonitoringPageModel> GetHistoryTransfersAsync(GetHistoryTransfersParam param);
        Task<Result> CancelJobAsync(int custId, int creator, string ip, string languageKey);
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
            // return new Result { Code = 1, Message = "error" };
            var isAutoJobRunning = await GetJobStatus(custId, creator, state);
            var criteria = new UpdateMultipleCriteria(creator, custId, isAutoJobRunning, language, ip, state);

            await repository.ExecuteAsync(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? string.Empty : criteria.ErrMsg
            };
        }

        public async Task<AutoTransferMonitoringPageModel> GetTodayTransfersAsync(GetTodayTransfersParam param)
        {
            var criteria = new GetTodayTransfersCriteria
            {
                CustId = param.CustId,
                Username = param.Username,
                Status = param.Status,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                SortedBy = param.SortedBy,
                Direction = param.Direction,
                TotalItems = 48
            };

            //  var transfers = await repository.FetchAsync<AutoTransferMonitoringModel>(criteria);


            return new AutoTransferMonitoringPageModel
            {
                Transfers = new List<AutoTransferMonitoringModel>
                {
                    new AutoTransferMonitoringModel
                    {
                        Username="Master01",
                        FirstLastName="ma1",
                        TransferAmount=325000,
                        Status="Completed",
                        Reason=string.Empty,
                        DateTime="01/05/2023 1:00:07 AM",
                        CustId=1
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master02",
                        FirstLastName="ma2",
                        TransferAmount=1800,
                        Status="Failed",
                        Reason="Due to system maintenance",
                        DateTime="01/05/2023 1:00:08 AM",
                        CustId=1
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master03",
                        FirstLastName="cloe",
                        TransferAmount=2800,
                        Status="Failed",
                        Reason="Due to unexpected issue",
                        DateTime="01/05/2023 1:00:08 AM",
                        CustId=1
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master04",
                        FirstLastName="da",
                        TransferAmount=3000,
                        Status="Failed",
                        Reason="Due to manual transfer",
                        DateTime="01/05/2023 1:00:08 AM",
                        CustId=1
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master05",
                        FirstLastName="dud",
                        TransferAmount=(decimal)2000.21,
                        Status="Processing",
                        Reason=string.Empty,
                        DateTime="01/05/2023 1:00:09 AM",
                        CustId=1
                    }
                },
                TotalItems = criteria.TotalItems,
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
                CustId = param.CustId,
                Username = param.Username,
                Status = param.Status,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                SortedBy = param.SortedBy,
                Direction = param.Direction,
                TotalItems = 48,
                FromDate = param.FromDate,
                ToDate = param.ToDate,
            };

            //  var transfers = await repository.FetchAsync<AutoTransferMonitoringModel>(criteria);


            return new AutoTransferMonitoringPageModel
            {
                Transfers = new List<AutoTransferMonitoringModel>
                {
                    new AutoTransferMonitoringModel
                    {
                        Username="Master01",
                        FirstLastName="ma1",
                        TransferAmount=325000,
                        Status="Completed",
                        Reason=string.Empty,
                        DateTime="01/05/2023 1:00:07 AM"
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master02",
                        FirstLastName="ma2",
                        TransferAmount=1800,
                        Status="Failed",
                        Reason="Due to system maintenance",
                        DateTime="01/05/2023 1:00:08 AM"
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master03",
                        FirstLastName="cloe",
                        TransferAmount=2800,
                        Status="Failed",
                        Reason="Due to unexpected issue",
                        DateTime="01/05/2023 1:00:08 AM"
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master04",
                        FirstLastName="da",
                        TransferAmount=3000,
                        Status="Failed",
                        Reason="Due to manual transfer",
                        DateTime="01/05/2023 1:00:08 AM"
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master05",
                        FirstLastName="dud",
                        TransferAmount=(decimal)2000.21,
                        Status="Processing",
                        Reason=string.Empty,
                        DateTime="01/05/2023 1:00:09 AM"
                    }
                },
                TotalItems = criteria.TotalItems,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                SortedBy = param.SortedBy,
                Direction = param.Direction
            };
        }

        public async Task<Result> CancelJobAsync(int custId, int creator, string ip, string languageKey)
        {
            //return new Result { Code = 1, Message = "error" };

            var criteria = new CancelJobCriteria(creator, custId, languageKey, ip);

            await repository.ExecuteAsync(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? string.Empty : criteria.ErrMsg
            };
        }

        private async Task<bool> GetJobStatus(int custId, int creator, string state)
        {
            var criteria = new GetJobStatusCriteria(custId, creator, state);

            await repository.ExecuteAsync(criteria);

            return criteria.IsAutoJobRunning;
        }
    }
}
