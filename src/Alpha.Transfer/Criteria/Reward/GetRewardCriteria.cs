using System;
using Alpha.Transfer.Models;
using Fanex.Data;
using Fanex.Data.Repository;

namespace Alpha.Transfer.Criteria.Reward
{
    internal abstract class GetRewardCriteria : CriteriaBase
    {
        protected GetRewardCriteria(
            int custId,
            UserLevel custLevel,
            DateTime fromDate,
            DateTime toDate,
            string programIds,
            bool? paymentStatus,
            string username)
        {
            CustId = custId;
            CustLevel = custLevel;
            FromDate = fromDate;
            ToDate = toDate;
            ProgramIds = programIds;
            PaymentStatus = paymentStatus;
            Username = username;
        }

        public int CustId { get; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string ProgramIds { get; }

        public bool? PaymentStatus { get; }

        public string Username { get; }

        public override bool IsValid() => CustId > 0 && FromDate <= ToDate && !string.IsNullOrWhiteSpace(ProgramIds);
    }
}
