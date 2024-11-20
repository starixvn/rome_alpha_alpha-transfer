namespace Alpha.Transfer.Models.TransferAllAndClearCredit
{
    using System;

    [Serializable]
    public class ClearCreditStep
    {
        public ClearCreditStep()
        {
        }

        public ClearCreditStep(int id, decimal givenCredit, decimal clearableCredit)
        {
            Id = id;
            GivenCredit = givenCredit;
            ClearableCredit = clearableCredit;
        }

        public int Id { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal ClearableCredit { get; set; }
    }
}
