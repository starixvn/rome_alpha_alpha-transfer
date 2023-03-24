namespace Alpha.Transfer.Site.Requests.AutoTransfer
{
    using System.Web.Mvc;
    using Alpha.Transfer.Models.AutoTransfer;
    using MediatR;
    using Transfer.Models;

    public class GetTodayTransfersQuery : IAsyncRequest<AutoTransferMonitoringPageModel>
    {
        public GetTodayTransfersQuery(IUser user)
        {
            CustId = user.Id;
        }

        public int CustId { get; }

        [AllowHtml]
        public string Username { get; set; }

        public string Status { get; set; }

        public int SortedBy { get; set; }

        public int Direction { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public bool IsValid() => CustId > 0;
    }
}