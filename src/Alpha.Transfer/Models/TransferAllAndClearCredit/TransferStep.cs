namespace Alpha.Transfer.Models.TransferAllAndClearCredit
{
    using System;

    [Serializable]
    public class TransferStep
    {
        public TransferStep()
        {
        }

        public TransferStep(int id, decimal transferableAmount, decimal? outstanding)
        {
            Id = id;
            TransferableAmount = transferableAmount;
            Outstanding = outstanding;
        }

        public int Id { get; set; }

        public decimal TransferableAmount { get; set; }

        public decimal? Outstanding { get; set; }
    }
}
