namespace Alpha.Transfer.Services
{
    using Criteria.MultipleTransfer;
    using DataAccesses;
    using Models.MultipleTransfer;
    using Params.MultipleTransfer;

    public interface MultipleTransferService
    {
        CustomerListModel GetCustomerList(GetCustomerListParam criteria);
    }

    public class MultipleTransferServiceImpl : MultipleTransferService
    {
        private readonly MultipleTransferDataAccess multipleTransferDataAccess;

        public MultipleTransferServiceImpl(MultipleTransferDataAccess multipleTransferDataAccess)
        {
            this.multipleTransferDataAccess = multipleTransferDataAccess;
        }

        public CustomerListModel GetCustomerList(GetCustomerListParam param)
        {
            var criteria = CreateCriteria(param);
            var data = multipleTransferDataAccess.GetCustomerList(criteria);
            return data;
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