namespace Alpha.Transfer.Site.Requests.AutoTransfer
{
    using MediatR;
    using Transfer.Models;

    public class UpdateMultipleQuery : IAsyncRequest<Result>
    {
        public UpdateMultipleQuery(IUser user)
        {
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            Language = user.LanguageKey;
        }

        public string SearchAccountIds { get; set; }

        public string Usernames { get; set; }

        public string State { get; set; }

        public int UplineId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string Language { get; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(SearchAccountIds);
    }
}