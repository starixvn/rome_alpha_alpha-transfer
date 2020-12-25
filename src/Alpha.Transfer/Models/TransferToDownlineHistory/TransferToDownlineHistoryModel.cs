namespace Alpha.Transfer.Models.TransferToDownlineHistory
{
    using System;

    public class TransferToDownlineHistoryModel
    {
        public TransferToDownlineHistoryModel(
            DateTime actionDate,
            string downlineName,
            string transferType,
            decimal transferAmount,
            decimal oldBalance,
            decimal newBalance,
            string description)
        {
            ActionDate = actionDate;
            DownlineName = downlineName;
            TransferType = transferType;
            TransferAmount = transferAmount;
            OldBalance = oldBalance;
            NewBalance = newBalance;
            Description = description;
        }

        public DateTime ActionDate { get; set; }

        public string DownlineName { get; set; }

        public string TransferType { get; set; }

        public decimal TransferAmount { get; set; }

        public decimal OldBalance { get; set; }

        public decimal NewBalance { get; set; }

        public string Description { get; set; }
    }
}