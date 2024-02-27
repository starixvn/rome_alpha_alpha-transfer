namespace Alpha.Transfer.Site.Requests.AutoTransfer
{
    using MediatR;
    using Transfer.Models;

    public class CancelJobQuery : IAsyncRequest<Result>
    {
        public CancelJobQuery(IUser user)
        {
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            LanguageKey = user.LanguageKey;
            AGIP = user.AGIP;
        }

        public int CustId { get; set; }

        public string ViewStatus { get; set; }

        public int UplineId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public string AGIP { get; }

        public bool IsValid() => CustId > 0 && UplineId > 0;
    }
}