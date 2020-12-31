namespace Alpha.Transfer.Site.Requests.TransferToDownlineHistory
{
    using Alpha.Transfer.Models;
    using MediatR;
    using ViewModels;

    public class TransferToDownlineHistoryExcelQuery : TransferToDownlineHistoryQuery, IRequest<ExcelFileViewModel>
    {
        public TransferToDownlineHistoryExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}