namespace Alpha.Transfer.Site.Requests.AutoTransfer
{
    using Alpha.Transfer.Site.ViewModels.AutoTransfer;
    using MediatR;

    public class GetTransferConditionQuery : IAsyncRequest<GetTransferConditionViewModel>
    {
        public GetTransferConditionQuery(UpdateMultipleQuery message)
        {
            UplineId = message.UplineId;
            ViewerLevel = message.ViewerLevel;
            SearchAccountIds = message.SearchAccountIds;
            Usernames = message.Usernames;
        }

        public int UplineId { get; set; }

        public byte ViewerLevel { get; set; }

        public string SearchAccountIds { get; set; }

        public string Usernames { get; set; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(SearchAccountIds);
    }
}