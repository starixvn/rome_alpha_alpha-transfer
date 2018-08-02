namespace Alpha.Transfer.Site.Handlers
{
    using System.Threading.Tasks;
    using MediatR;
    using Params.MultipleTransfer;
    using Requests.MultipleTransfer;
    using Services;
    using ViewModels.MultipleTransfer;

    public class MultipleTransferHandler :
        IRequestHandler<CustomerListQuery, CustomerListViewModel>
    {
        private readonly MultipleTransferService multipleTransferService;

        public MultipleTransferHandler(MultipleTransferService multipleTransferService)
        {
            this.multipleTransferService = multipleTransferService;
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

            var data = multipleTransferService.GetCustomerList(param);

            return new CustomerListViewModel(message, data);
        }
    }
}