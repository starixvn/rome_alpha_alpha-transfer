namespace Alpha.Transfer.Site.Requests.TransferAllAndClearCredit
{
    using System.Web.Mvc;
    using Configurations;
    using MediatR;
    using Transfer.Models;
    using ViewModels.TransferAllAndClearCredit;

    public class CalculationQuery : TransferBaseQuery, IRequest<CalculationViewModel>
    {
        public CalculationQuery(IUser viewer) : base(viewer)
        {
            IsInternal = viewer.IsInternal;
            LanguageKey = viewer.LanguageKey;
            IsAllDayBalanceTransfer = viewer.AllowAllDayBalanceTransfer;
        }

        [AllowHtml]
        public string Username { get; set; }

        public bool IsInternal { get; }

        public string AllowedTotalBalanceTransferSuperIds => AppSettings.AllowedTotalBalanceTransferSuperIds;

        public bool IsAllDayBalanceTransfer { get; }

        public string LanguageKey { get; }
    }
}
