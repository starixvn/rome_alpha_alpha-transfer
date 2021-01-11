namespace Alpha.Transfer.Site.Handlers
{
    using Alpha.Transfer.AppResources;
    using Alpha.Transfer.Extensions;
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Alpha.Transfer.Services;
    using Alpha.Transfer.Utils;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Requests.TransferToDownlineHistory;
    using ViewModels;

    public class TransferToDownlineHistoryExcelHandler : TransferToDownlineHistoryBaseHandler, IRequestHandler<TransferToDownlineHistoryExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<TransferToDownlineHistoryModel> workbook;

        public TransferToDownlineHistoryExcelHandler(
            TransferToDownlineHistoryService transferToDownlineHistoryService,
            Workbook<TransferToDownlineHistoryModel> workbook)
            : base(transferToDownlineHistoryService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(TransferToDownlineHistoryExcelQuery message)
        {
            var content = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetSheetTitle(BuildTransferToDownlineHistoryParam(message)))
                .Header(Resources.ResourceManager.GetString)
                .Body(GetTransferToDownlineHistory(message), GetBodyMapper)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(), content);
        }

        protected internal Mapper<TransferToDownlineHistoryModel> GetBodyMapper
            => new Mapper<TransferToDownlineHistoryModel>()
            .Map(Column.A, m => m.ActionDate.ToLongDateWithSlash24HFormat())
            .Map(Column.B, m => m.DownlineName)
            .Map(Column.C, m => m.TransferType)
            .Map(Column.D, m => m.TransferAmount)
            .Map(Column.E, m => m.OldBalance)
            .Map(Column.F, m => m.NewBalance)
            .Map(Column.G, m => m.Description);

        private TemplateInfo GetTemplateInfo(TransferToDownlineHistoryExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                Resources.NoInformationIsAvailable);

        private string GetSheetTitle(TransferToDownlineHistoryParam param)
        {
            string userName = string.IsNullOrWhiteSpace(param.UserName)
                ? string.Empty
                : string.Format(" - Username: {0}", param.UserName);

            return $"{Resources.TransferToDownlineHistory} {param.FromDate.ToShortDateWithSlash()} -> {param.ToDate.ToShortDateWithSlash()}{userName}";
        }

        private string GetExcelFileName() => $"{Resources.TransferToDownlineHistory} - {SystemTime.Today.ToShortDateString()}";
    }
}