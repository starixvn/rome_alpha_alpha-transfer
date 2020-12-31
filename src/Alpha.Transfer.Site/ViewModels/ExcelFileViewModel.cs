namespace Alpha.Transfer.Site.ViewModels
{
    using System.IO;
    using System.Linq;

    public class ExcelFileViewModel
    {
        public ExcelFileViewModel(string name, byte[] content)
        {
            Name = GetValidFileName(name);
            Content = content;
            ContentType = "application/vnd.ms-excel";
        }

        public string Name { get; }

        public byte[] Content { get; }

        public string ContentType { get; }

        private static string GetValidFileName(string fileName)
            => string.IsNullOrWhiteSpace(fileName)
            ? "no-name.xls"
            : Path.GetInvalidFileNameChars().Aggregate(
                fileName,
                (current, c) => current.Replace(c.ToString(), string.Empty)) + ".xls";
    }
}