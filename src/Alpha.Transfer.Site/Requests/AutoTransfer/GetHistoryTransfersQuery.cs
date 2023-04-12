namespace Alpha.Transfer.Site.Requests.AutoTransfer
{
    using System;
    using System.Web.Mvc;
    using Alpha.Transfer.Models.AutoTransfer;
    using MediatR;
    using Transfer.Models;

    public class GetHistoryTransfersQuery : IAsyncRequest<AutoTransferMonitoringPageModel>
    {
        public GetHistoryTransfersQuery(IUser user)
        {
            CustId = user.Id;
            Language = user.LanguageKey;
            LimitedDateToShowInfo = user.LimitedDateToShowInfo;
        }

        public int CustId { get; }

        public string Language { get; set; }

        public DateTime LimitedDateToShowInfo { get; }

        [AllowHtml]
        public string Username { get; set; }

        public string Status { get; set; }

        public int SortedBy { get; set; }

        public int Direction { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsValid() => CustId > 0;
    }
}