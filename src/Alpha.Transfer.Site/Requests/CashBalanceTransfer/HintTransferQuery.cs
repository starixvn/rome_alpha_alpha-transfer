namespace Alpha.Transfer.Site.Requests.CashBalanceTransfer
{
    using MediatR;
    using Transfer.Models;
    using ViewModels.CashBalanceTransfer;

    public class HintTransferQuery : IAsyncRequest<HintTransferViewModel>
    {
        public HintTransferQuery(IUser user)
        {
            CustId = user.Id;
            RoleId = user.LevelId;
            Language = user.Language;
            Currency = user.Currency;
            User = user;
        }

        public IUser User { get; }

        public int CustId { get; }

        public byte RoleId { get; }

        public string Language { get; }

        public string Currency { get; }

        public bool IsValid() => CustId > 0;
    }
}