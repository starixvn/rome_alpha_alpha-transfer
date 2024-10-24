using System.Collections.Generic;
using Alpha.Transfer.Models;
using Alpha.Transfer.Models.Reward;
using Alpha.Transfer.Site.Requests.Reward;

namespace Alpha.Transfer.Site.ViewModels.Reward
{
    public class RewardPaymentViewModel
    {
        public RewardPaymentViewModel(
            RewardBaseQuery message,
            IEnumerable<RewardProgram> programs,
            RewardPayment reportData)
        {
            ActorLevel = message.ActorLevel;
            Filter = new RewardFilterViewModel(message, programs);
            ReportData = reportData;
        }

        public UserLevel ActorLevel { get; }

        public RewardFilterViewModel Filter { get; }

        public RewardPayment ReportData { get; }
    }
}