using Alpha.Transfer.Models;
using Alpha.Transfer.Models.Reward;

namespace Alpha.Transfer.Site.ViewModels.Reward
{
    public class RewardDetailDataViewModel
    {
        public RewardDetailDataViewModel(UserLevel actorLevel, RewardDetail reportData)
        {
            ActorLevel = actorLevel;
            ReportData = reportData;
        }

        public UserLevel ActorLevel { get; }

        public RewardDetail ReportData { get; }
    }
}