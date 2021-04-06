namespace Alpha.Transfer.Site.Requests.TransferToDownlineHistory
{
    using Alpha.Transfer.Configurations;
    using Alpha.Transfer.Models;
    using System;
    using System.Web.Mvc;

    public class TransferToDownlineHistoryBaseQuery : TransferBaseQuery
    {
        public TransferToDownlineHistoryBaseQuery(IUser viewer) : base(viewer)
        {
            LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
            IsAllDayBalTransfer = viewer.IsAllDayBalTransfer;
            Site = viewer.Site;
        }

        [AllowHtml]
        public string UserName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime LimitedDateToShowInfo { get; }

        public DateTime ToDate { get; set; }

        public int UserSelectedDateRangeOption { get; set; }

        public bool IsAllDayBalTransfer { get; }

        public string Site { get; }

        public override bool IsValid()
            => base.IsValid()
            && AppSettings.IsAllowedTransferToDownlineHistorySite(Site)
            && IsAllDayBalTransfer;
    }
}