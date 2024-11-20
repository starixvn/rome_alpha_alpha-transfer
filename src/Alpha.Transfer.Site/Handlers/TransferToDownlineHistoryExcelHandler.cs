namespace Alpha.Transfer.Site.Handlers
{
    using Alpha.Transfer.AppResources;
    using Alpha.Transfer.Extensions;
    using Alpha.Transfer.Models;
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Alpha.Transfer.Services;
    using Alpha.Transfer.Utils;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Requests.TransferToDownlineHistory;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

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
            var param = new TransferToDownlineHistoryPagingParam(
                message.CustId,
                message.UserName,
                message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                message.ToDate,
                message.LevelType,
                0,
                0);
            var content = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetSheetTitle(param))
                .Header(Resources.ResourceManager.GetString)
                .Body(GetTransferToDownlineHistory(param).Item1, GetBodyMapper(message.CustLevel))
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(), content);
        }

        protected internal Mapper<TransferToDownlineHistoryModel> GetBodyMapper(UserLevel custLevel)
        {
            var mapper = new Mapper<TransferToDownlineHistoryModel>()
                .Map(A, m => m.FormattedActionDate)
                .Map(B, m => m.DownlineName);
            if (custLevel.IsAgentLevel())
            {
                mapper
                    .Map(C, m => m.TransferTypeName)
                    .Map(D, m => m.FormattedTransferAmount)
                    .Map(E, m => m.FormattedOldBalance)
                    .Map(F, m => m.FormattedNewBalance)
                    .Map(G, m => m.Description);
            }
            else
            {
                mapper
                    .Map(C, m => m.IsDirectMember ? Resources.DirectMember : (custLevel.IsSuperLevel() ? Resources.MasterAgent : Resources.Agent))
                    .Map(D, m => m.TransferTypeName)
                    .Map(E, m => m.FormattedTransferAmount)
                    .Map(F, m => m.FormattedOldBalance)
                    .Map(G, m => m.FormattedNewBalance)
                    .Map(H, m => m.Description);
            }

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(TransferToDownlineHistoryExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath($"TransferToDownlineHistory/{message.CustLevel.GetDirectDownLineLevel().DisplayName}"),
                Resources.NoInformationIsAvailable);

        private string GetSheetTitle(TransferToDownlineHistoryParam param)
        {
            string userName = string.IsNullOrWhiteSpace(param.UserName)
                ? string.Empty
                : $" - {Resources.Username}: {param.UserName}";

            return $"{Resources.TransferToDownlineHistory} {param.FromDate.ToShortDateWithSlash()} -> {param.ToDate.ToShortDateWithSlash()}{userName}";
        }

        private string GetExcelFileName() => $"{Resources.TransferToDownlineHistory} - {SystemTime.Today.ToShortDateString()}";
    }
}