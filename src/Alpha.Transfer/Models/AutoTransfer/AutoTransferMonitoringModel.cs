namespace Alpha.Transfer.Models.AutoTransfer
{
    public class AutoTransferMonitoringModel
    {
        public string Username { get; set; }
        public string FirstLastName { get; set; }
        public decimal? TransferAmount { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string DateTime { get; set; }
        public int CustId { get; set; }
    }
}
