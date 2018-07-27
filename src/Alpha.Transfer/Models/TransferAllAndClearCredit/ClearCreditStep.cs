namespace Alpha.Transfer.Models.TransferAllAndClearCredit
{
    using System;

    [Serializable]
    public class ClearCreditStep
    {
        public ClearCreditStep()
        {
        }

        public ClearCreditStep(string level, decimal givenCredit, decimal clearableCredit)
        {
            Level = level;
            GivenCredit = givenCredit;
            ClearableCredit = clearableCredit;
        }

        public string Level { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal ClearableCredit { get; set; }

        public decimal OldClearableCredit { get; set; }

        public bool HasChanged => OldClearableCredit != ClearableCredit;
    }
}
