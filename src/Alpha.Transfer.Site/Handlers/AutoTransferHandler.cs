namespace Alpha.Transfer.Site.Handlers
{
    using System.Threading.Tasks;
    using Alpha.Transfer.AppResources;
    using Alpha.Transfer.Models.AutoTransfer;
    using Alpha.Transfer.Params.AutoTransfer;
    using Alpha.Transfer.Services;
    using Alpha.Transfer.Site.Requests.AutoTransfer;
    using Alpha.Transfer.Site.ViewModels.AutoTransfer;
    using MediatR;
    using Transfer.Models;

    public class AutoTransferHandler :
        IAsyncRequestHandler<GetTransferConditionQuery, GetTransferConditionViewModel>,
        IAsyncRequestHandler<UpdateQuery, Result>,
        IAsyncRequestHandler<GetTodayTransfersQuery, AutoTransferMonitoringPageModel>,
        IAsyncRequestHandler<GetHistoryTransfersQuery, AutoTransferMonitoringPageModel>,
        IAsyncRequestHandler<CancelJobQuery, Result>,
        IAsyncRequestHandler<CancelAllJobsQuery, Result>,
        IAsyncRequestHandler<PostUpdateScheduleQuery, Result>,
        IAsyncRequestHandler<PostUpdateMultipleAutoTransferQuery, UpdateMultipleAutoTransferResultModel>
    {
        private readonly AutoTransferService service;
        private readonly CashBalanceTransferService cashBalanceTransferService;

        public AutoTransferHandler(
            AutoTransferService service,
            CashBalanceTransferService cashBalanceTransferService)
        {
            this.service = service;
            this.cashBalanceTransferService = cashBalanceTransferService;
        }

        public async Task<GetTransferConditionViewModel> Handle(GetTransferConditionQuery message)
        {
            var tuple = (await cashBalanceTransferService.GetTransferCondition(message.UplineId));
            var transferCondition = tuple.Item1 ? Resources.Daily : string.Join(", ", tuple.Item2);

            return new GetTransferConditionViewModel
            {
                ViewerLevel = message.ViewerLevel,
                SearchAccountIds = message.SearchAccountIds,
                Usernames = message.Usernames,
                TransferCondition = transferCondition
            };
        }

        public async Task<Result> Handle(UpdateQuery message)
        {
            var param = new UpdateSingleParam
            {
                CustId = message.CustId,
                UplineId = message.UplineId,
                IP = message.IP,
                Language = message.Language,
                State = message.State,
                Schedule = message.DateSchedule,
                SubAccId = message.SubAccId,
                Old_TransferOption = message.Old_TransferOption,
                AGIP = message.AGIP
            };

            var result = await service.UpdateAsync(param);

            var successMessage =
                string.Equals("on", message.State, System.StringComparison.OrdinalIgnoreCase)
                ? AppResources.Resources.AutoTransfer_UpdateSuccessOn
                : AppResources.Resources.AutoTransfer_UpdateSuccessOff;

            return new Result
            {
                Code = result.Code == 0 && !string.IsNullOrWhiteSpace(result.Message) ? -1 : result.Code,
                Message = string.IsNullOrWhiteSpace(result.Message)
                    ? string.Format(successMessage, message.CustName)
                    : result.Message
            };
        }

        public async Task<AutoTransferMonitoringPageModel> Handle(GetTodayTransfersQuery message)
        {
            var param = new GetTodayTransfersParam
            {
                CustId = message.CustId,
                Language = message.Language,
                Username = message.Username,
                Status = message.Status,
                PageSize = message.PageSize,
                PageIndex = message.PageIndex,
                SortedBy = message.SortedBy,
                Direction = message.Direction,
                AGIP = message.AGIP
            };

            return await service.GetTodayTransfersAsync(param);
        }

        public async Task<AutoTransferMonitoringPageModel> Handle(GetHistoryTransfersQuery message)
        {
            var param = new GetHistoryTransfersParam
            {
                CustId = message.CustId,
                Language = message.Language,
                Username = message.Username,
                Status = message.Status,
                PageSize = message.PageSize,
                PageIndex = message.PageIndex,
                SortedBy = message.SortedBy,
                Direction = message.Direction,
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                LimitedDateToShowInfo = message.LimitedDateToShowInfo
            };

            return await service.GetHistoryTransfersAsync(param);
        }

        public async Task<Result> Handle(CancelJobQuery message)
        {
            var param = new CancelJobParam
            {
                FromCustId = message.UplineId,
                ToCustId = message.CustId,
                ViewStatus = message.ViewStatus,
                Language = message.LanguageKey,
                IP = message.IP,
                AGIP = message.AGIP
            };

            return await service.CancelJobAsync(param);
        }

        public async Task<Result> Handle(CancelAllJobsQuery message)
        {
            var param = new CancelAllJobParam
            {
                FromCustId = message.UplineId,
                Language = message.LanguageKey,
                IP = message.IP,
                AGIP = message.AGIP
            };

            var result = await service.CancelAllJobAsync(param);

            return new Result
            {
                Code = result.Code,
                Message = result.Code == 0 ? Resources.AutoTransfer_CancelAllSuccess : result.Message
            };
        }

        public async Task<Result> Handle(PostUpdateScheduleQuery message)
        {
            var param = new UpdateScheduleParam
            {
                CustId = message.CustId,
                CustName = message.CustName,
                Schedule = message.Schedule,

                UplineId = message.UplineId,
                SubAccId = message.SubAccId,
                IP = message.IP,
                Language = message.Language,
                Old_TransferOption = message.Old_TransferOption,
                AGIP = message.AGIP
            };

            var result = await service.UpdateScheduleAsync(param);

            return new Result
            {
                Code = result.Code,
                Message = result.Code == 0 ? string.Format(Resources.AutoTransfer_UpdateScheduleSuccess, message.CustName) : result.Message
            };
        }

        public async Task<UpdateMultipleAutoTransferResultModel> Handle(PostUpdateMultipleAutoTransferQuery message)
        {
            var param = new UpdateMultipleAutoTransferParam
            {
                UplineId = message.UplineId,
                SubAccId = message.SubAccId,
                IP = message.IP,
                Language = message.Language,

                OnIds = message.OnIds,
                OffIds = message.OffIds,
                OnStatus = message.OnStatus,
                OffStatus = message.OffStatus,
                OnSchedule = message.OnSchedule,
                OffSchedule = message.OffSchedule,
                Old_TransferOption = message.Old_TransferOption,
                OnNames = message.OnNames,
                OffNames = message.OffNames,
                AGIP = message.AGIP
            };

            return await service.UpdateMultipleAutoTransferAsync(param);
        }
    }
}