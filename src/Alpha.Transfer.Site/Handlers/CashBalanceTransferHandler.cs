namespace Alpha.Transfer.Site.Handlers
{
    using System.Threading.Tasks;
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
        private readonly CashBalanceTransferService service;

        public CashBalanceTransferHandler(CashBalanceTransferService service)
        {
            this.service = service;
        }

        public async Task<CashBalanceTransferViewModel> Handle(CashBalanceTransferQuery message)
        {
            var model = await service.GetCashBalanceTransferAsync(message.CustId);

            return new CashBalanceTransferViewModel(
                model.UplineCashBalance,
                model.CustId,
                model.UserName,
                Enumeration.FromValue<UserLevel>(model.RoleId),
                model.CashBalance,
                model.Outstanding,
                model.GivenCredit);
        }

        public async Task<Result> Handle(CashBalanceTransferCommand message)
        {
            return await service.AddCashBalanceTransferAsync(
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
            string allowTransfer = string.Empty;
            var userRole = Enumeration.FromValue<UserLevel>(message.RoleId);

            if (userRole.IsNotSuperLevel())
            {
                allowTransfer = await service.GetAllowTransfer(message.CustId);
            }

            return new HintTransferViewModel(allowTransfer, message.ShowWarningTransfer);
        }
    }
}