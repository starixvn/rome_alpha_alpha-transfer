namespace Alpha.Transfer.Site.Handlers
{
    using Alpha.Transfer.Extensions;
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Alpha.Transfer.Services;
    using Requests.TransferToDownlineHistory;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;

    public class TransferToDownlineHistoryBaseHandler
    {
        private readonly TransferToDownlineHistoryService transferToDownlineHistoryService;

        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/TransferToDownlineHistory/{0}.{1}";

        private Func<string, string> mapPath;

        protected internal Func<string, string> MapPath
        {
            get { return mapPath ?? HttpContext.Current.Server.MapPath; }
            set { mapPath = value; }
        }

        protected internal Func<string, bool> IsFileExist { get; set; } = File.Exists;

        public TransferToDownlineHistoryBaseHandler(TransferToDownlineHistoryService transferToDownlineHistoryService)
        {
            this.transferToDownlineHistoryService = transferToDownlineHistoryService;
        }

        protected internal IEnumerable<TransferToDownlineHistoryModel> GetTransferToDownlineHistory(TransferToDownlineHistoryQuery message)
            => transferToDownlineHistoryService.GetTransferToDownlineHistory(BuildTransferToDownlineHistoryParam(message));

        protected internal TransferToDownlineHistoryParam BuildTransferToDownlineHistoryParam(TransferToDownlineHistoryQuery message)
            => new TransferToDownlineHistoryParam(
                message.CustId,
                message.UserName,
                message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                message.ToDate);

        protected internal virtual string GetExcelTemplatePath(string templateName, string fileExtension = "xls")
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName), "Template Name cannot be null or empty!");
            }

            var relativePath = string.Format(ExcelTemplateDir, templateName, fileExtension);

            var absolutePath = MapPath(relativePath);

            if (IsFileExist(absolutePath))
            {
                return absolutePath;
            }

            throw new FileNotFoundException($"Template {absolutePath} does not exist!");
        }
    }
}