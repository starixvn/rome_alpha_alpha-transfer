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

        public string TransferTypeName
            => TransferType == 1 ? Resources.Deposit : Resources.Withdrawal;

        public byte TransferType { get; }

        public decimal TransferAmount { get; }

        public string FormattedTransferAmount
            => TransferAmount.FormatWithCommasAlwaysWithDecimalPoint();

        public decimal OldBalance { get; }

        public string FormattedOldBalance
            => OldBalance.FormatWithCommasAlwaysWithDecimalPoint();

        public decimal NewBalance { get; }

        public string FormattedNewBalance
            => NewBalance.FormatWithCommasAlwaysWithDecimalPoint();

        public string Description { get; }
    }
}