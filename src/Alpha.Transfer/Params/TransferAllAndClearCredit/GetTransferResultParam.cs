namespace Alpha.Transfer.Params.TransferAllAndClearCredit
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.TransferAllAndClearCredit;
    using Newtonsoft.Json;

    public class GetTransferResultParam
    {
        public GetTransferResultParam(
            int custId,
            int userId,
            int subAccId,
            bool isClearCredit,
            string username,
            string allowedTotalBalanceTransferSuperIds,
            string ip,
            string language,
            string transferSteps,
            string clearCreditSteps,
            bool isAllDayBalanceTransfer,
            string agIP)
        {
            CustId = custId;
            UserId = userId;
            SubAccId = subAccId;
            IsClearCredit = isClearCredit;
            Username = username;
            AllowedTotalBalanceTransferSuperIds = allowedTotalBalanceTransferSuperIds;
            IP = ip;
            Language = language;
            IsClearCredit = isClearCredit;
            IsAllDayBalanceTransfer = isAllDayBalanceTransfer;
            TransferSteps = JsonConvert.DeserializeObject<IEnumerable<TransferStep>>(transferSteps.Replace("'", "\""))
                ?? Enumerable.Empty<TransferStep>();
            ClearCreditSteps = JsonConvert.DeserializeObject<IEnumerable<ClearCreditStep>>(clearCreditSteps.Replace("'", "\""))
                ?? Enumerable.Empty<ClearCreditStep>();
            AGIP = agIP;
        }

        public int CustId { get; }

        public int UserId { get; }

        public int SubAccId { get; }

        public bool IsClearCredit { get; }

        public string Username { get; }

        public string AllowedTotalBalanceTransferSuperIds { get; }

        public string IP { get; }

        public string Language { get; }

        public bool IsAllDayBalanceTransfer { get; }

        public IEnumerable<TransferStep> TransferSteps { get; }

        public IEnumerable<ClearCreditStep> ClearCreditSteps { get; }

        public bool SkipTransfer() => !TransferSteps.Any(s => s.TransferableAmount != 0);

        public string AGIP { get; }
    }
}
