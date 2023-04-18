namespace Alpha.Transfer.Site.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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
            var result = await cashBalanceTransferService.GetTransferCondition(message.UplineId);

            return new GetTransferConditionViewModel
            {
                SearchAccountIds = message.SearchAccountIds,
                Usernames = message.Usernames,
                TransferCondition = result
            };
        }

        public async Task<Result> Handle(UpdateMultipleQuery message)
        {
            var ids = message.SearchAccountIds.Split(new[] { ',' });
            var resultMessages = new List<string>();

            foreach (var id in ids)
            {
                int custId;
                if (!int.TryParse(id, out custId) || custId == 0)
                {
                    continue;
                }

                var result = await service.UpdateAsync(custId, message.UplineId, message.IP, message.Language, message.State);

                if (result.Code != 0)
                {
                    resultMessages.Add(result.Message);
                }
            }

            return new Result
            {
                Code = resultMessages.Any() ? 1 : 0,
                Message = resultMessages.Any() ? string.Join("<br />", resultMessages) : AppResources.Resources.AutoTransfer_UpdateSuccess
            };
        }

        public async Task<Result> Handle(UpdateQuery message)
        {
            var result = await service.UpdateAsync(message.CustId, message.UplineId, message.IP, message.Language, message.State);

            return new Result
            {
                Code = result.Code,
                Message = result.Code == 0 ? AppResources.Resources.AutoTransfer_UpdateSuccess : result.Message
            };
        }

        public async Task<AutoTransferMonitoringPageModel> Handle(GetTodayTransfersQuery message)
        {
            var param = new GetTodayTransfersParam
            {
                CustId = message.CustId,
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
                Username = message.Username,
                Status = message.Status,
                PageSize = message.PageSize,
                PageIndex = message.PageIndex,
                SortedBy = message.SortedBy,
                Direction = message.Direction,
                FromDate = message.FromDate,
                ToDate = message.ToDate
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