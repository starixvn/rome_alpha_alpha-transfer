namespace Alpha.Transfer.Site.Requests
{
    using System.Collections.Generic;
    using Alpha.Transfer.Models;
    using Alpha.Transfer.Models.Customer;
    using MediatR;

    public class UsernameSuggestionQuery : TransferBaseQuery, IAsyncRequest<IEnumerable<UsernameSuggestion>>
    {
        public UsernameSuggestionQuery(IUser viewer) : base(viewer)
        {
            IsInternal = viewer.IsInternal;
        }

        public string Query { get; set; }

        public bool DirectDownlines { get; set; }

        public bool IsAccInfo { get; set; } = true;

        public bool IsInternal { get; }

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Query) && base.IsValid();
    }
}