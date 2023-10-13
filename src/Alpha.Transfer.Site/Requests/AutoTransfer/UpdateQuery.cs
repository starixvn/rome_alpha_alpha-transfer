namespace Alpha.Transfer.Site.Requests.AutoTransfer
{
    using System;
    using System.Linq;
    using MediatR;
    using Transfer.Models;

    public class UpdateQuery : IAsyncRequest<Result>
    {
        public UpdateQuery(IUser user)
        {
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            Language = user.LanguageKey;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public string State { get; set; }

        public string Schedule { get; set; }

        public int UplineId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string Language { get; }

        public string[] DateSchedule => (Schedule ?? string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        public bool IsValid() => CustId > 0 && UplineId > 0 && (State == "off" || DateSchedule.Any());
    }
}