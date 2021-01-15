namespace Alpha.Transfer.Models.TransferToDownlineHistory
{
    using AppResources;
    using System;

    public class TransferToDownlineHistoryModel
    {
        public TransferToDownlineHistoryModel()
        {
        }

        public TransferToDownlineHistoryModel(
            DateTime actionDate,
            string downlineName,
            byte transferType,
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

        public DateTime ActionDate { get; }

        public string DownlineName { get; }

        public string TransferTypeName
            => TransferType == 0 ? Resources.Deposit : Resources.Withdrawal;

        public byte TransferType { get; }

        public decimal TransferAmount { get; }

        public decimal OldBalance { get; }

        public decimal NewBalance { get; }

        public string Description { get; }
    }
}