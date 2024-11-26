using System.Collections.Generic;

namespace Alpha.Transfer.Models.Reward
{
    public class RewardDetail
    {
        public RewardDetail(
            IEnumerable<RewardDetailEntity> rewardDetails,
            int pageIndex,
            int pageSize,
            int totalRecords)
        {
            RewardDetails = rewardDetails;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            Any = totalRecords > 0;
            MoreThanOne = totalRecords > 1;
        }

        public IEnumerable<RewardDetailEntity> RewardDetails { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRecords { get; }

        public bool Any { get; }

        public bool MoreThanOne { get; }
    }
}
