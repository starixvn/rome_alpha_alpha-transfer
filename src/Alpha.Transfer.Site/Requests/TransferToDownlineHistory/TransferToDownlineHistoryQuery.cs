namespace Alpha.Transfer.Site.Requests.TransferToDownlineHistory
{
    using Alpha.Transfer.Models;
    using MediatR;
    using System;
    using ViewModels.TransferToDownlineHistory;

    public class TransferToDownlineHistoryQuery : TransferBaseQuery, IRequest<TransferToDownlineHistoryViewModel>
    {
        public TransferToDownlineHistoryQuery(IUser viewer) : base(viewer)
        {
        }

        public string UserName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int UserSelectedDateRangeOption { get; set; }
    }
}