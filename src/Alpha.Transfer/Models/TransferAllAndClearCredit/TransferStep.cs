namespace Alpha.Transfer.Models.TransferAllAndClearCredit
{
    using System;

    [Serializable]
    public class TransferStep
    {
        public TransferStep()
        {
        }

        public TransferStep(string name, decimal transferableAmount, decimal? outstanding)
        {
            Name = name;
            TransferableAmount = transferableAmount;
            Outstanding = outstanding;
        }

        public string Name { get; set; }

        public decimal TransferableAmount { get; set; }

        public decimal? Outstanding { get; set; }

        public decimal? OldOutstanding { get; set; }

        public bool HasChanged => OldOutstanding.HasValue && OldOutstanding.Value != Outstanding.Value;
    }
}
