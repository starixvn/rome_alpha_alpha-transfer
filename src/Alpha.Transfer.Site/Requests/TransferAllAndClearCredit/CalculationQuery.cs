namespace Alpha.Transfer.Site.Requests.TransferAllAndClearCredit
{
    using MediatR;
    using Transfer.Models;
    using ViewModels.TransferAllAndClearCredit;

    public class CalculationQuery : TransferBaseQuery, IAsyncRequest<CalculationViewModel>
    {
        public CalculationQuery(IUser viewer) : base(viewer)
        {
        }

        public string Username { get; set; }
    }
}
