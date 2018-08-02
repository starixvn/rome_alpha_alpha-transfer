namespace Alpha.Transfer.Params.MultipleTransfer
{
    using Models;

    public class GetCustomerListParam
    {
        public GetCustomerListParam(
            UserLevel userLevel,
            int custId,
            string username,
            int pageIndex,
            int pageSize,
            bool allYesterdayBalance,
            bool allowTotalBalanceTransfer,
            int status)
        {
            UserLevel = userLevel;
            CustId = custId;
            Username = username;
            PageIndex = pageIndex;
            PageSize = pageSize;
            AllYesterdayBalance = allYesterdayBalance;
            AllowTotalBalanceTransfer = allowTotalBalanceTransfer;
            Status = status;
        }

        public UserLevel UserLevel { get; }

        public int CustId { get; }

        public string Username { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public bool AllYesterdayBalance { get; }

        public bool AllowTotalBalanceTransfer { get; }

        public int Status { get; }
    }
}
