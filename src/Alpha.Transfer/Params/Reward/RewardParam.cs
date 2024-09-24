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
            int? programId,
            bool? paymentStatus,
            string username,
            int pageIndex = 0,
            int pageSize = 0)
        {
            CustId = custId;
            CustLevel = custLevel;
            FromDate = fromDate;
            ToDate = toDate;
            ProgramId = programId;
            PaymentStatus = paymentStatus;
            Username = username;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int? ProgramId { get; }

        public bool? PaymentStatus { get; }

        public string Username { get; }

        public int PageIndex { get; }

        public int PageSize { get; }
    }
}
