namespace Alpha.Transfer.Site.Handlers
{
    using MediatR;
    using Params.TransferAllAndClearCredit;
    using Requests.TransferAllAndClearCredit;
    using Services;
    using Transfer.Models.TransferAllAndClearCredit;
    using ViewModels.TransferAllAndClearCredit;

    public class TransferAllAndClearCreditHandler :
        IRequestHandler<CalculationQuery, CalculationViewModel>,
        IRequestHandler<TransferResultQuery, TransferResultViewModel>
    {
        private readonly TransferAllAndClearCreditService transferAllAndClearCreditService;

        public TransferAllAndClearCreditHandler(TransferAllAndClearCreditService transferAllAndClearCreditService)
        {
            this.transferAllAndClearCreditService = transferAllAndClearCreditService;
        }

        public CalculationViewModel Handle(CalculationQuery message)
        {
            var calculationResult =
                string.IsNullOrWhiteSpace(message.Username)
                ? new TransferResult()
                : transferAllAndClearCreditService.GetCalculateResult(message.CustId, message.Username);

            return new CalculationViewModel(message, calculationResult);
        }

        public TransferResultViewModel Handle(TransferResultQuery message)
        {
            var param = new GetTransferResultParam(
                message.CustId,
                message.UserId,
                message.SubAccId,
                message.IsClearCredit,
                message.Username,
                message.AllowedTotalBalanceTransferSuperIds,
                message.IP,
                message.LanguageKey,
                message.TransferSteps,
                message.ClearCreditSteps,
                message.IsAllDayBalanceTransfer);

            var transferResult = message.IsInternal ? null : transferAllAndClearCreditService.GetTransferResult(param);

            return new TransferResultViewModel(message, param, transferResult);
        }
    }
}