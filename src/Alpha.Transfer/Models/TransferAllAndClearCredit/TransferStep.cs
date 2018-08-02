namespace Alpha.Transfer.Models.TransferAllAndClearCredit
{
    using System;

    [Serializable]
    public class TransferStep
    {
        public TransferStep()
        {
        }

        public TransferStep(byte id, decimal transferableAmount, decimal? outstanding)
        {
            Id = id;
            TransferableAmount = transferableAmount;
            Outstanding = outstanding;
        }

        public byte Id { get; set; }

        public decimal TransferableAmount { get; set; }

        public decimal? Outstanding { get; set; }
    }
}
