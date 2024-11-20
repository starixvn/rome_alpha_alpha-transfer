using System;
using Alpha.Transfer.AppResources;

namespace Alpha.Transfer.Models.Reward
{
    public class RewardDetailEntity
    {
        public DateTime CreatedDate { get; set; }

        public int ProgramId { get; set; }

        public string ProgramName { get; set; }

        public int CustId { get; set; }

        public string MemberUsername { get; set; }

        public decimal MemberAmount { get; set; }

        public bool MemberPaymentStatus { get; set; }

        public DateTime? MemberPaymentDate { get; set; }

        public byte? MemberFeedback { get; set; }

        public string MemberFeedbackText
            => MemberFeedback == 0 ? Resources.Received : MemberFeedback == 1 ? Resources.NotReceived : string.Empty;

        public int Recommend { get; set; }

        public string AgentUsername { get; set; }

        public bool AgentPaymentStatus { get; set; }

        public DateTime? AgentPaymentDate { get; set; }

        public int MRecommend { get; set; }

        public string MasterUsername { get; set; }

        public bool MasterPaymentStatus { get; set; }

        public DateTime? MasterPaymentDate { get; set; }

        public bool IsDirectMember { get; set; }
    }
}
