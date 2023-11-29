namespace Alpha.Transfer.DataAccesses
{
    using System.Collections.Generic;
    using Criteria.MultipleTransfer;
    using Fanex.Common;
    using Fanex.Data;
    using Models.MultipleTransfer;

    public interface MultipleTransferDataAccess
    {
        CustomerListModel GetCustomerList(GetCustomerListCriteria criteria);
    }

    public class MultipleTransferDataAccessImpl : MultipleTransferDataAccess
    {
        public CustomerListModel GetCustomerList(GetCustomerListCriteria criteria)
        {
            var customerList = new List<CustomerInformation>();
            var noOfCustomer = 0;

            using (var objectDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            customerList.Add(new CustomerInformation
                            {
                                Username = Converter.ToString2(reader["username"]),
                                YesterdayBalance = Converter.ToDecimal(reader["yesterday_magentBalance"]),
                                Balance = Converter.ToDecimal(reader["magentbalance"]),
                                YesterdayCashBalance = Converter.ToDecimal(reader["yesterday_magentCashBalance"]),
                                CashBalance = Converter.ToDecimal(reader["magentCashBalance"]),
                                Outstanding = Converter.ToDecimal(reader["magentOutstanding"]),
                                GivenCredit = Converter.ToDecimal(reader["magentCredit"]),
                                UsedCredit = Converter.ToDecimal(reader["magentUsedCredit"]),
                                FirstName = Converter.ToString2(reader["firstname"]),
                                LastName = Converter.ToString2(reader["lastname"]),
                                CustId = Converter.ToInt(reader["custid"]),
                                Enabled = Converter.ToBool(reader["enabled"]),
                                Closed = Converter.ToBool(reader["closed"]),
                                Suspended = Converter.ToBool(reader["suspended"]),
                                ClosedReason = Converter.ToString2(reader["closedreason"])
                            });
                        }

                        reader.Close();
                    }

                    var output = objectDb.GetOutputParams();
                    noOfCustomer = Converter.ToInt(output["TotalPages"]);
                }
            }

            return new CustomerListModel(customerList, noOfCustomer);
        }
    }
}