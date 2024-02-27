namespace Alpha.Transfer.Services
{
    using Criteria.MultipleTransfer;
    using Fanex.Data.Repository;
    using Models.MultipleTransfer;
    using Params.MultipleTransfer;

    public interface MultipleTransferService
    {
        CustomerListModel GetCustomerList(GetCustomerListParam criteria);
    }

    public class MultipleTransferServiceImpl : MultipleTransferService
    {
        private readonly IDynamicRepository repository;

        public MultipleTransferServiceImpl(IDynamicRepository repository)
            => this.repository = repository;

        public CustomerListModel GetCustomerList(GetCustomerListParam param)
        {
            var criteria = CreateCriteria(param);
            var data = repository.Fetch<CustomerInformation>(criteria);

            return new CustomerListModel(data, criteria.TotalPages);
        }

        private static GetCustomerListCriteria CreateCriteria(GetCustomerListParam param)
        {
            if (param.UserLevel.IsAgentLevel())
            {
                return new GetAgentTransferListCriteria(param);
            }

            if (param.UserLevel.IsMasterLevel())
            {
                return new GetMasterTransferListCriteria(param);
            }

            return new GetSuperTransferListCriteria(param);
        }
    }
}