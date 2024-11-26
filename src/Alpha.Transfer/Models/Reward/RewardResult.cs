using System.Collections.Generic;

namespace Alpha.Transfer.Models.Reward
{
    public class RewardResult
    {
        public RewardResult(bool isKickout)
        {
            IsKickout = isKickout;
        }

        public RewardResult(int totalRecords, IEnumerable<string> errors)
        {
            TotalRecords = totalRecords;
            Errors = errors;
        }

        public bool IsKickout { get; }

        public int TotalRecords { get; }

        public IEnumerable<string> Errors { get; }
    }
}
