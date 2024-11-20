namespace Alpha.Transfer.Params.TransferAllAndClearCredit
{
    public class GetCalculationResultParam
    {
        public GetCalculationResultParam(
            int custId,
            string username,
            string language,
            bool isAllDayBalanceTransfer,
            string agIP)
        {
            CustId = custId;
            Username = username;
            Language = language;
            IsAllDayBalanceTransfer = isAllDayBalanceTransfer;
            AGIP = agIP;
        }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string Language { get; set; }

        public bool IsAllDayBalanceTransfer { get; set; }

        public string AGIP { get; set; }
    }
}
