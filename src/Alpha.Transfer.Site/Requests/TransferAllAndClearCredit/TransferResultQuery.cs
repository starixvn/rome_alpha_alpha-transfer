namespace Alpha.Transfer.Site.Requests.TransferAllAndClearCredit
{
    using System.Collections.Generic;
    using MediatR;
    using Transfer.Models;
    using Transfer.Models.TransferAllAndClearCredit;
    using ViewModels.TransferAllAndClearCredit;

    public class TransferResultQuery : TransferBaseQuery, IAsyncRequest<TransferResultViewModel>
    {
        public TransferResultQuery(IUser viewer) : base(viewer)
        {
        }

        public string Username { get; set; }

        public bool IsClearCredit { get; set; }

        public string TransferSteps { get; set; }

        public string ClearCreditSteps { get; set; }
    }
}