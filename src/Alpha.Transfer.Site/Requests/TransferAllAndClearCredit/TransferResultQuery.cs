namespace Alpha.Transfer.Site.Requests.TransferAllAndClearCredit
{
    using System.Web.Mvc;
    using Configurations;
    using MediatR;
    using Transfer.Models;
    using ViewModels.TransferAllAndClearCredit;

    public class TransferResultQuery : TransferBaseQuery, IRequest<TransferResultViewModel>
    {
        public TransferResultQuery(IUser viewer) : base(viewer)
        {
            UserId = viewer.UserId;
            SubAccId = viewer.SubAccId;
            IP = viewer.IP;
            LanguageKey = viewer.LanguageKey;
            IsInternal = viewer.IsInternal;
        }

        public int UserId { get; }

        public int SubAccId { get; }

        public bool IsClearCredit { get; set; }

        [AllowHtml]
        public string Username { get; set; }

        public string AllowedTotalBalanceTransferSuperIds => AppSettings.AllowedTotalBalanceTransferSuperIds;

        public string IP { get; set; }

        public string TransferSteps { get; set; }

        public string ClearCreditSteps { get; set; }

        public string LanguageKey { get; set; }

        public bool IsInternal { get; }
    }
}