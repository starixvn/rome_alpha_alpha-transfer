using System;
using Alpha.Transfer.Models;

namespace Alpha.Transfer.Criteria.Reward
{
    internal class GetRewardDetailCriteria : GetRewardPaymentCriteria
    {
        public GetRewardDetailCriteria(
            int custId,
            UserLevel custLevel,
            DateTime fromDate,
            DateTime toDate,
            int? programId,
            bool? paymentStatus,
            string username,
            int pageIndex, 
            int pageSize) : base(custId, custLevel, fromDate, toDate, programId, paymentStatus, username)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRecords { get; set; }

        public override string GetSettingKey()
            => CustLevel.IsSuperLevel()
            ? "Age_Rpt_RewardDetail_Master"
            : CustLevel.IsMasterLevel()
                ? "Age_Rpt_RewardDetail_Agent"
                : "Age_Rpt_RewardDetail_Member";
    }
}
