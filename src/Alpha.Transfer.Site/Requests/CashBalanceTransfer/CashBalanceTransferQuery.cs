namespace Alpha.Transfer.Site.Requests.CashBalanceTransfer
{
    using MediatR;
    using ViewModels.CashBalanceTransfer;

    public class CashBalanceTransferQuery : IAsyncRequest<CashBalanceTransferViewModel>
    {
        public int CustId { get; set; }

        public bool IsValid() => CustId > 0;
    }
}