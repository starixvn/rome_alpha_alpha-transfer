namespace Alpha.Transfer.Site.Requests.AutoTransfer
{
    using System.Web.Mvc;
    using MediatR;
    using Transfer.Models;

    public class CancelAllJobsQuery : IAsyncRequest<Result>
    {
        public CancelAllJobsQuery(IUser user)
        {
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            LanguageKey = user.LanguageKey;
            AGIP = user.AGIP;
        }

        public int UplineId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public string AGIP { get; }

        [AllowHtml]
        public string Username { get; set; }

        public string Status { get; set; }

        public bool IsValid() => UplineId > 0;
    }
}