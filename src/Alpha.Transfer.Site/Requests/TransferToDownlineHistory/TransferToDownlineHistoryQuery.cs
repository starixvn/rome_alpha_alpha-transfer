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
        }

        [AllowHtml]
        public string UserName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int UserSelectedDateRangeOption { get; set; }
    }
}