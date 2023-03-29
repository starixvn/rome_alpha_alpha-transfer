namespace Alpha.Transfer.Site.Requests.CashBalanceTransfer
{
    using Alpha.Transfer.Models;
    using MediatR;
    using ViewModels.CashBalanceTransfer;

    public class CashBalanceTransferQuery : IAsyncRequest<CashBalanceTransferViewModel>
    {
        public CashBalanceTransferQuery(IUser user)
        {
            UplineId = user.Id;
        }

        public int CustId { get; set; }

        public int UplineId { get; }

        public bool IsValid() => CustId > 0;
    }
}