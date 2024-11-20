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
            AGIP = user.AGIP;
            ActorLevel = Enumeration.FromValue<UserLevel>(user.LevelId);
        }

        public int CustId { get; set; }

        public bool IsDirectMember { get; set; }

        public int UplineId { get; }

        public string AGIP { get; }

        public UserLevel ActorLevel { get; }

        public bool IsValid() => CustId > 0;
    }
}