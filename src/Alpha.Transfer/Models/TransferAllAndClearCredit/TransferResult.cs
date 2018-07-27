namespace Alpha.Transfer.Models.TransferAllAndClearCredit
{
    using System.Collections.Generic;

    public class TransferResult
    {
        public TransferResult(
            bool isSuccess,
            string errorMessage,
            IEnumerable<TransferStep> transferSteps,
            IEnumerable<ClearCreditStep> clearCreditSteps)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            TransferSteps = transferSteps;
            ClearCreditSteps = clearCreditSteps;
        }

        public bool IsSuccess { get; }

        public string ErrorMessage { get; }

        public IEnumerable<TransferStep> TransferSteps { get; }

        public IEnumerable<ClearCreditStep> ClearCreditSteps { get; }

        public bool HasDataChanged()
        {
            foreach (var step in TransferSteps)
            {
                if (step.HasChanged)
                {
                    return true;
                }
            }

            foreach (var step in TransferSteps)
            {
                if (step.HasChanged)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
