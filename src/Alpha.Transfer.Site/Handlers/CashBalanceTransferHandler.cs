namespace Alpha.Transfer.Site.Handlers
{
    using System.Threading.Tasks;
    using Alpha.Transfer.AppResources;
    using Alpha.Transfer.Configurations;
    using MediatR;
    using Requests.CashBalanceTransfer;
    using Services;
    using Transfer.Models;
    using ViewModels.CashBalanceTransfer;

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
            var model = await cashBalanceTransferService.GetCashBalanceTransferAsync(message.CustId, message.IsDirectMember, message.AGIP);

            return new CashBalanceTransferViewModel(
                message.ActorLevel,
                model.UplineCashBalance,
                model.CustId,
                model.UserName,
                Enumeration.FromValue<UserLevel>(model.RoleId),
                model.CashBalance,
                model.Outstanding,
                model.GivenCredit,
                model.PendingTransfer3rd,
                message.IsDirectMember);
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
                message.LanguageKey,
                message.AGIP,
                message.IsDirectMember);
        }

        public async Task<HintTransferViewModel> Handle(HintTransferQuery message)
        {
            var userRole = Enumeration.FromValue<UserLevel>(message.RoleId);

            var allowTransfer = userRole.IsNotSuperLevel()
                ? await cashBalanceTransferService.GetAllowTransfer(message.CustId)
                : string.Empty;
            var isAllDayBalTransfer = await transferService.IsAllDayBalanceTransferAsync(message.CustId);
            var isEnabledAutoTransfer = !transferService.IsAllDayBalanceTransfer(message.CustId) && transferService.IsEnableAutoTransfer(message.User.Id, message.User.Currency, message.User.IsSubAcc, message.User.SubPageAccess, AppSettings.GetConfigValue("OfferAutoTransfer.Currencies"));

            return new HintTransferViewModel(allowTransfer, isAllDayBalTransfer, isEnabledAutoTransfer);
        }
    }
}