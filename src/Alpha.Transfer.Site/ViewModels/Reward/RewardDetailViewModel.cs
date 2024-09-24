using System.Collections.Generic;
using Alpha.Transfer.Models.Reward;
using Alpha.Transfer.Site.Requests.Reward;

namespace Alpha.Transfer.Site.ViewModels.Reward
{
    public class RewardDetailViewModel : RewardDetailDataViewModel
    {
        public RewardDetailViewModel(
            RewardBaseQuery message,
            IEnumerable<RewardProgram> programs,
            RewardDetail reportData) : base(message.ActorLevel, reportData)
        {
            Filter = new RewardFilterViewModel(message, programs, showColumns: true);
            VisibleColumnKey = $"RewardDetail_{message.ViewerId}_{message.SubAccId}:Columns";
        }

        public RewardFilterViewModel Filter { get; }

        public string VisibleColumnKey { get; }
    }
}