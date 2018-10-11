namespace Alpha.Transfer.Site.Requests.MultipleTransfer
{
    using Constants;
    using MediatR;
    using Transfer.Models;
    using ViewModels.MultipleTransfer;

    public class CustomerListQuery : TransferBaseQuery, IRequest<CustomerListViewModel>
    {
        public CustomerListQuery(IUser viewer) : base(viewer)
        {
            AllowAllDayBalanceTransfer = viewer.AllowAllDayBalanceTransfer;
            AllowTotalBalanceTransfer = viewer.AllowTotalBalanceTransfer;
        }

        public string Username { get; set; }

        public int Status { get; set; } = StatusConstant.Open;

        public bool AllYesterdayBalance { get; set; }

        public bool AllowAllDayBalanceTransfer { get; }

        public bool AllowTotalBalanceTransfer { get; }

        public int PageSize { get; set; } = PagerConstant.DefaultPageSize;

        public int PageIndex { get; set; } = PagerConstant.DefaultPageIndex;
    }
}