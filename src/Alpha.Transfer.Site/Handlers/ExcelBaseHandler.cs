using System;
using System.IO;
using System.Web;

namespace Alpha.Transfer.Site.Handlers
{
    public abstract class ExcelBaseHandler
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/{0}.{1}";
        private Func<string, string> mapPath;

        protected internal Func<string, string> MapPath
        {
            get { return mapPath ?? HttpContext.Current.Server.MapPath; }
            set { mapPath = value; }
        }

        internal Func<string, bool> IsFileExist { get; set; } = File.Exists;

        protected internal virtual string GetExcelTemplatePath(string folderAndFileName, string fileExtension = "xls")
        {
            if (string.IsNullOrWhiteSpace(folderAndFileName))
            {
                throw new ArgumentNullException(nameof(folderAndFileName), "Template Name cannot be null or empty");
            }

            var relativePath = string.Format(ExcelTemplateDir, folderAndFileName, fileExtension);
            var absolutePath = MapPath(relativePath);

            if (IsFileExist(absolutePath))
            {
                return absolutePath;
            }

            throw new FileNotFoundException($"Template {absolutePath} does not exist");
        }
    }
}
