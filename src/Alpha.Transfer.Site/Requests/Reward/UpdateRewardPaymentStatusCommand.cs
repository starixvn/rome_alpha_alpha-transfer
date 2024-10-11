using System;
using Alpha.Transfer.Models;
using Alpha.Transfer.Models.Reward;
using MediatR;

namespace Alpha.Transfer.Site.Requests.Reward
{
    public class UpdateRewardPaymentStatusCommand : TransferBaseQuery, IAsyncRequest<RewardResult>
    {
        public UpdateRewardPaymentStatusCommand(IUser viewer) : base(viewer)
        {
            LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
            SubAccId = viewer.SubAccId;
            SubAccName = viewer.SubAccName;
            IP = viewer.IP;
            LanguageKey = viewer.LanguageKey;
            IsInternal = viewer.IsInternal;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime LimitedDateToShowInfo { get; }

        public RewardPaymentStatus[] Data { get; set; }

        public int SubAccId { get; }

        public string SubAccName { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public bool IsInternal { get; }

        public override bool IsValid()
            => !IsInternal
            && Data != null && Data.Length > 0
            && FromDate != DateTime.MinValue && ToDate != DateTime.MinValue && FromDate <= ToDate;
    }
}