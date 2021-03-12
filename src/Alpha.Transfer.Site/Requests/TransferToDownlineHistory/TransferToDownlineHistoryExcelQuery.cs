namespace Alpha.Transfer.Site.Requests.TransferToDownlineHistory
{
    using Alpha.Transfer.Models;
    using MediatR;
    using ViewModels;

    public class TransferToDownlineHistoryExcelQuery : TransferToDownlineHistoryBaseQuery, IRequest<ExcelFileViewModel>
    {
        public TransferToDownlineHistoryExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}