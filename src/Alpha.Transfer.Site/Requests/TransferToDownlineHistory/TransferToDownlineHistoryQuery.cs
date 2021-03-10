namespace Alpha.Transfer.Site.Requests.TransferToDownlineHistory
{
    using Alpha.Transfer.Models;
    using MediatR;
    using System;
    using System.Web.Mvc;
    using ViewModels.TransferToDownlineHistory;

    public class TransferToDownlineHistoryQuery : TransferBaseQuery, IRequest<TransferToDownlineHistoryViewModel>
    {
        public TransferToDownlineHistoryQuery(IUser viewer) : base(viewer)
        {
            LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
        }

        [AllowHtml]
        public string UserName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime LimitedDateToShowInfo { get; set; }

        public DateTime ToDate { get; set; }

        public int UserSelectedDateRangeOption { get; set; }
    }
}