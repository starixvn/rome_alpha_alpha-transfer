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
        }

        public int CustId { get; set; }

        public int UplineId { get; }

        public string ViewStatus { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public bool IsValid() => CustId > 0 && UplineId > 0;
    }
}