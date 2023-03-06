namespace Alpha.Transfer.Site.Handlers
{
    using MediatR;
    using Params.MultipleTransfer;
    using Requests.MultipleTransfer;
    using Services;
    using ViewModels.MultipleTransfer;

    public class MultipleTransferHandler :
        IRequestHandler<CustomerListQuery, CustomerListViewModel>
    {
        private readonly MultipleTransferService multipleTransferService;
        private readonly TransferService transferService;

        public MultipleTransferHandler(
            MultipleTransferService multipleTransferService,
            TransferService transferService)
        {
            this.multipleTransferService = multipleTransferService;
            this.transferService = transferService;
        }

        public CustomerListViewModel Handle(CustomerListQuery message)
        {
            var param = new GetCustomerListParam(
                message.CustLevel,
                message.CustId,
                message.Username,
                message.PageIndex,
                message.PageSize,
                message.AllYesterdayBalance,
                message.AllowTotalBalanceTransfer,
                message.Status);

            var isAllDayBalanceTransfer = transferService.IsAllDayBalanceTransfer(message.ViewerId);
            var data = multipleTransferService.GetCustomerList(param);

            return new CustomerListViewModel(message, data, isAllDayBalanceTransfer);
        }
    }
}