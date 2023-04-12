namespace Alpha.Transfer.Site.Handlers
{
    using System.Threading.Tasks;
    using Alpha.Transfer.AppResources;
    using MediatR;
    using Requests.CashBalanceTransfer;
    using Services;
    using Transfer.Models;
    using ViewModels.CashBalanceTransfer;
    using Alpha.Transfer.Configurations;

    public class CashBalanceTransferHandler :
        IAsyncRequestHandler<CashBalanceTransferQuery, CashBalanceTransferViewModel>,
        IAsyncRequestHandler<CashBalanceTransferCommand, Result>,
        IAsyncRequestHandler<HintTransferQuery, HintTransferViewModel>
    {
        private readonly CashBalanceTransferService cashBalanceTransferService;
        private readonly TransferService transferService;

        public CashBalanceTransferHandler(
            CashBalanceTransferService cashBalanceTransferService,
            TransferService transferService)
        {
            this.cashBalanceTransferService = cashBalanceTransferService;
            this.transferService = transferService;
        }

        public async Task<CashBalanceTransferViewModel> Handle(CashBalanceTransferQuery message)
        {
            var model = await cashBalanceTransferService.GetCashBalanceTransferAsync(message.CustId);

            return new CashBalanceTransferViewModel(
                model.UplineCashBalance,
                model.CustId,
                model.UserName,
                Enumeration.FromValue<UserLevel>(model.RoleId),
                model.CashBalance,
                model.Outstanding,
                model.GivenCredit,
                model.PendingTransfer3rd);
        }

        public async Task<Result> Handle(CashBalanceTransferCommand message)
        {
            var isAllDayBalTransfer = await transferService.IsAllDayBalanceTransferAsync(message.UplineId);
            if (!isAllDayBalTransfer)
            {
                return Result.CreateErrorResult(Resources.DisallowedAllDayBalanceTransferError, 6001);
            }

            return await cashBalanceTransferService.AddCashBalanceTransferAsync(
                message.UplineId,
                message.CustId,
                message.Amount,
                message.UplineId,
                message.UplineName,
                message.SubAccId,
                message.IP,
                message.LanguageKey);
        }

        public async Task<HintTransferViewModel> Handle(HintTransferQuery message)
        {
            var userRole = Enumeration.FromValue<UserLevel>(message.RoleId);

            var allowTransfer = userRole.IsNotSuperLevel()
                ? await cashBalanceTransferService.GetAllowTransfer(message.CustId)
                : string.Empty;
            var isAllDayBalTransfer = await transferService.IsAllDayBalanceTransferAsync(message.CustId);
            var isEnabledAutoTransfer = AppSettings.IsEnableAutoTransfer(message.User.Currency, message.User.IsSubAcc, message.User.SubPageAccess);

            return new HintTransferViewModel(allowTransfer, isAllDayBalTransfer, isEnabledAutoTransfer);
        }
    }
}