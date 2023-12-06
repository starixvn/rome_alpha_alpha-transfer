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
            Language = user.LanguageKey;
            AGIP = user.AGIP;
        }

        public int CustId { get; }

        public string Language { get; set; }

        [AllowHtml]
        public string Username { get; set; }

        public string Status { get; set; }

        public int SortedBy { get; set; }

        public int Direction { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string AGIP { get; }

        public bool IsValid() => CustId > 0;
    }
}