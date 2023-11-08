namespace Alpha.Transfer.Site.Requests.AutoTransfer
{
    using Alpha.Transfer.Models.AutoTransfer;
    using MediatR;
    using Transfer.Models;

    public class PostUpdateMultipleAutoTransferQuery : IAsyncRequest<UpdateMultipleAutoTransferResultModel>
    {
        public PostUpdateMultipleAutoTransferQuery(IUser user)
        {
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            Language = user.LanguageKey;
        }

        public int UplineId { get; }
        public int SubAccId { get; }
        public string IP { get; }
        public string Language { get; }

        public string OnIds { get; set; }
        public string OffIds { get; set; }
        public string OnStatus { get; set; }
        public string OffStatus { get; set; }
        public string OnSchedule { get; set; }
        public string OffSchedule { get; set; }
        public string Old_TransferOption { get; set; }
        public string OnNames { get; set; }
        public string OffNames { get; set; }
    }
}