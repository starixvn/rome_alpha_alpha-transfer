namespace Alpha.Transfer.Site.Handlers
{
    using System.Threading.Tasks;
    using MediatR;
    using Params.TransferAllAndClearCredit;
    using Requests.TransferAllAndClearCredit;
    using Services;
    using ViewModels.TransferAllAndClearCredit;

    public class TransferAllAndClearCreditHandler :
        IAsyncRequestHandler<CalculationQuery, CalculationViewModel>,
        IAsyncRequestHandler<TransferResultQuery, TransferResultViewModel>
    {
        private readonly TransferAllAndClearCreditService transferAllAndClearCreditService;

        public TransferAllAndClearCreditHandler(TransferAllAndClearCreditService transferAllAndClearCreditService)
        {
            this.transferAllAndClearCreditService = transferAllAndClearCreditService;
        }

        public async Task<CalculationViewModel> Handle(CalculationQuery message)
        {
            var transferSteps = transferAllAndClearCreditService.GetTransferSteps(message.Username);
            var clearCreditSteps = transferAllAndClearCreditService.GetClearCreditSteps(message.Username);

            return new CalculationViewModel(message, transferSteps, clearCreditSteps);
        }

        public async Task<TransferResultViewModel> Handle(TransferResultQuery message)
        {
            var param = new GetTransferResultParam(
                message.Username,
                message.IsClearCredit,
                message.TransferSteps,
                message.ClearCreditSteps);

            var transferResult = transferAllAndClearCreditService.GetTransferResult(param);

            return new TransferResultViewModel(message, param, transferResult);
        }
    }
}