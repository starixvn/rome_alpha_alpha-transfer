namespace Alpha.Transfer.Site.Handlers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Transfer.AppResources;
    using Alpha.Transfer.Extensions;
    using Alpha.Transfer.Models.AutoTransfer;
    using Alpha.Transfer.Params.AutoTransfer;
    using Alpha.Transfer.Services;
    using Alpha.Transfer.Site.Requests.AutoTransfer;
    using Alpha.Transfer.Site.ViewModels.AutoTransfer;
    using MediatR;
    using Transfer.Models;

    public class AutoTransferHandler :
        IAsyncRequestHandler<GetTransferConditionQuery, GetTransferConditionViewModel>,
        IAsyncRequestHandler<UpdateMultipleQuery, Result>,
        IAsyncRequestHandler<UpdateQuery, Result>,
        IAsyncRequestHandler<GetTodayTransfersQuery, AutoTransferMonitoringPageModel>,
        IAsyncRequestHandler<GetHistoryTransfersQuery, AutoTransferMonitoringPageModel>,
        IAsyncRequestHandler<CancelJobQuery, Result>
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

        public async Task<Result> Handle(UpdateMultipleQuery message)
        {
            var ids = message.SearchAccountIds.Split(new[] { ',' });
            var names = message.Usernames.Split(new[] { ',' });

            var failedCusts = new List<string>();
            var isError = false;
            var isWarning = false;

            for (var i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var name = names[i];
                int custId;

                if (!int.TryParse(id, out custId) || custId == 0)
                {
                    continue;
                }

                var result = await service.UpdateAsync(custId, message.UplineId, message.IP, message.Language, message.State);

                if (result.Code != 0)
                {
                    failedCusts.Add(name);
                    isError = true;
                }
                else if (!string.IsNullOrWhiteSpace(result.Message))
                {
                    failedCusts.Add(name);
                    isWarning = true;
                }
            }

            var resultMessage = string.Empty;

            if (isError)
            {
                resultMessage = $"{(failedCusts.Count > 1 ? Resources.AutoTransfer_MultipleErrorProccessing : Resources.AutoTransfer_MultipleErrorProccessing_Single)} ({failedCusts.ToSeparatedString()})";
            }
            else if (isWarning)
            {
                resultMessage = $"{(failedCusts.Count > 1 ? Resources.AutoTransfer_MultipleAffectNextConditionWarning : Resources.AutoTransfer_MultipleAffectNextConditionWarning_Single)} ({failedCusts.ToSeparatedString()})";
            }

            return new Result
            {
                Code = isError ? 1 : 0,
                Message = resultMessage
            };
        }

        public async Task<Result> Handle(UpdateQuery message)
        {
            var result = await service.UpdateAsync(message.CustId, message.UplineId, message.IP, message.Language, message.State);
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
                Direction = message.Direction
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
            };

            return await service.CancelJobAsync(param);
        }
    }
}