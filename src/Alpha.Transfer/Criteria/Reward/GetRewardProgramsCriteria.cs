using Fanex.Data.Repository;

namespace Alpha.Transfer.Criteria.Reward
{
    internal class GetRewardProgramsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_Rpt_GetProgramList";

        public override bool IsValid() => true;
    }
}
