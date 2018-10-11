namespace Alpha.Transfer.Models.MultipleTransfer
{
    using System.Collections.Generic;

    public class CustomerListModel
    {
        public CustomerListModel(IEnumerable<CustomerInformation> customerList, int noOfCustomer)
        {
            CustomerList = customerList;
            NoOfCustomer = noOfCustomer;
        }

        public IEnumerable<CustomerInformation> CustomerList { get; }

        public int NoOfCustomer { get; }
    }
}
