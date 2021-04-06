namespace Alpha.Transfer.Models.TransferToDownlineHistory
{
    using Alpha.Transfer.Extensions;
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

        public string FormattedActionDate
            => ActionDate.ToLongDateWithSlash24HFormat();

        public string DownlineName { get; }

        public byte TransferType { get; }

        public string TransferTypeName
            => TransferType == 1 ? Resources.Deposit : Resources.Withdrawal;

        public decimal TransferAmount { get; }

        public string FormattedTransferAmount
            => TransferAmount.FormatWithCommasAlwaysWithDecimalPoint();

        public decimal OldBalance { get; }

        public string FormattedOldBalance => FormatBalance(OldBalance);

        public decimal NewBalance { get; }

        public string FormattedNewBalance => FormatBalance(NewBalance);

        public string Description { get; }

        private string FormatBalance(decimal balance)
            => TransferAmount != 0 && OldBalance == 0 && NewBalance == 0
            ? "N/A"
            : balance.FormatWithCommasAlwaysWithDecimalPoint();
    }
}