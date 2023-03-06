namespace Alpha.Transfer.Site.ViewModels.MultipleTransfer
{
    using System.Collections.Generic;
    using Constants;
    using Requests.MultipleTransfer;
    using Transfer.Models.MultipleTransfer;
    using Web.Helpers;

    public class CustomerListViewModel
    {
        public CustomerListViewModel(CustomerListQuery query, CustomerListModel model, bool allowAllDayBalanceTransfer)
        {
            Username = query.Username;
            Status = query.Status;
            AllYesterdayBalance = query.AllYesterdayBalance;
            AllowAllDayBalanceTransfer = allowAllDayBalanceTransfer;
            AllowTotalBalanceTransfer = query.AllowTotalBalanceTransfer;

            PageSize = query.PageSize;
            PageIndex = query.PageIndex;
            CustomerListModel = model;

            StatusOptions = new List<int> {
                StatusConstant.All,
                StatusConstant.Open,
                StatusConstant.Suspended,
                StatusConstant.Closed,
                StatusConstant.Disabled
            };
        }

        public string Username { get; }

        public int Status { get; }

        public IEnumerable<int> StatusOptions { get; }

        public bool AllYesterdayBalance { get; }

        public bool AllowAllDayBalanceTransfer { get; }

        public bool AllowTotalBalanceTransfer { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        public CustomerListModel CustomerListModel { get; }

        public string UsernameAutoSuggestionPath
            => MicroSites.OldMainUrl("~/_GlobalResources/Handlers/QueryUserName.ashx");

        public string TransferInformationPath
            => MicroSites.TransferUrl("~/cashbalancetransfer/hint");

        public bool CanTransferYesterday(CustomerInformation customer)
        {
            if (AllowAllDayBalanceTransfer)
            {
                return AllowTotalBalanceTransfer && customer.Balance != 0 && customer.YesterdayBalance != 0;
            }

            if (AllowTotalBalanceTransfer)
            {
                return customer.Balance != 0;
            }

            return customer.YesterdayBalance != 0;
        }

        public string GetYesterdayBalanceColor(CustomerInformation customer)
        {
            if (AllowAllDayBalanceTransfer)
            {
                if (AllowTotalBalanceTransfer && (customer.Balance == 0 || customer.YesterdayBalance == 0))
                {
                    return string.Empty;
                }

                if (customer.YesterdayBalance < 0)
                {
                    return "favorite";
                }
            }

            if (AllowTotalBalanceTransfer)
            {
                return string.Empty;
            }

            return customer.YesterdayBalance < 0 ? "favorite" : "underdog";
        }

        public string GetBalanceColor(CustomerInformation customer)
            => customer.Balance < 0 ? "favorite" : string.Empty;

        public string GetCashBalanceColor(decimal cashBalance) => cashBalance < 0 ? "favorite" : string.Empty;

        public bool ShowYesterdayBalanceLink(CustomerInformation customer)
            => !AllowAllDayBalanceTransfer && !AllowTotalBalanceTransfer && customer.YesterdayBalance != 0;

        public bool ShowBalanceLink(CustomerInformation customer)
            => !AllowAllDayBalanceTransfer && AllowTotalBalanceTransfer && customer.Balance != 0;

        public bool ShowCashBalanceLink(CustomerInformation customer)
            => AllowAllDayBalanceTransfer;
    }
}