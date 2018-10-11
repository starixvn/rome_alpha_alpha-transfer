namespace Alpha.Transfer.Site.ViewModels.TransferAllAndClearCredit
{
    using Web.Helpers;

    public static class TransferAllAndClearCreditViewModel
    {
        public static string GetOutstandingText(decimal? outstanding)
              => outstanding.HasValue ? outstanding.Value.ToString($"#,##0.00") : "N/A";

        public static string UsernameAutoSuggestionPath
            => MicroSites.OldMainUrl("~/_GlobalResources/Handlers/QueryUserName.ashx");
    }
}