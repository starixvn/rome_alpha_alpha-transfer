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
            IsAlldayTransfer = user.IsAllDayBalTransfer;
        }

        public int CustId { get; }

        public byte RoleId { get; }

        public string Language { get; }

        public bool ShowWarningTransfer { get; set; }

        public bool IsValid() => CustId > 0;

        public bool IsAlldayTransfer { get; }
    }
}