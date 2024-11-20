using System;
using Alpha.Transfer.Models;

namespace Alpha.Transfer.Params.Reward
{
    public class RewardParam
    {
        public RewardParam(
            int custId,
            UserLevel custLevel,
            DateTime fromDate,
            DateTime toDate,
            string programIds,
            bool? paymentStatus,
            byte levelType,
            string username,
            int pageIndex = 0,
            int pageSize = 0)
        {
            CustId = custId;
            CustLevel = custLevel;
            FromDate = fromDate;
            ToDate = toDate;
            ProgramIds = programIds;
            PaymentStatus = paymentStatus;
            LevelType = levelType;
            Username = username;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string ProgramIds { get; }

        public bool? PaymentStatus { get; }

        public byte LevelType { get; }

        public string Username { get; }

        public int PageIndex { get; }

        public int PageSize { get; }
    }
}
