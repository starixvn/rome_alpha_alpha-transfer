namespace Alpha.Transfer.Params.TransferAllAndClearCredit
{
    using System.Collections.Generic;
    using Models.TransferAllAndClearCredit;
    using Newtonsoft.Json;

    public class GetTransferResultParam
    {
        public GetTransferResultParam(
            string username,
            bool isClearCredit,
            string transferSteps,
            string clearCreditSteps)
        {
            Username = username;
            IsClearCredit = isClearCredit;
            TransferSteps = JsonConvert.DeserializeObject<IEnumerable<TransferStep>>(transferSteps);
            ClearCreditSteps = JsonConvert.DeserializeObject<IEnumerable<ClearCreditStep>>(clearCreditSteps);
        }

        public string Username { get; }

        public bool IsClearCredit { get; }

        public IEnumerable<TransferStep> TransferSteps { get; }

        public IEnumerable<ClearCreditStep> ClearCreditSteps { get; }
    }
}
