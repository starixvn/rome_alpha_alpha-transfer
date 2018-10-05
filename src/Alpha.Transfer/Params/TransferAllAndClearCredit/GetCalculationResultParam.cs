namespace Alpha.Transfer.Params.TransferAllAndClearCredit
{
    public class GetCalculationResultParam
    {
        public GetCalculationResultParam(
            int custId,
            string username,
            string language,
            string allowedTotalBalanceTransferSuperIds,
            bool isAllDayBalanceTransfer)
        {
            CustId = custId;
            Username = username;
            Language = language;
            AllowedTotalBalanceTransferSuperIds = allowedTotalBalanceTransferSuperIds;
            IsAllDayBalanceTransfer = isAllDayBalanceTransfer;
        }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string Language { get; set; }

        public string AllowedTotalBalanceTransferSuperIds { get; set; }

        public bool IsAllDayBalanceTransfer { get; set; }
    }
}
