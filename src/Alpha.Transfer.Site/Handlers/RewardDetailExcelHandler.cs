using System.Threading.Tasks;
using Alpha.Transfer.AppResources;
using Alpha.Transfer.Extensions;
using Alpha.Transfer.Models;
using Alpha.Transfer.Models.Reward;
using Alpha.Transfer.Services;
using Alpha.Transfer.Site.Requests.Reward;
using Alpha.Transfer.Site.ViewModels;
using MediatR;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace Alpha.Transfer.Site.Handlers
{
    public class RewardDetailExcelHandler : ExcelBaseHandler, IAsyncRequestHandler<RewardDetailExcelQuery, ExcelFileViewModel>
    {
        private readonly RewardService rewardService;
        private readonly Workbook<RewardDetailEntity> workbook;

        public RewardDetailExcelHandler(RewardService rewardService, Workbook<RewardDetailEntity> workbook)
        {
            this.rewardService = rewardService;
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(RewardDetailExcelQuery message)
        {
            var reportData = await rewardService.GetRewardDetails(RewardHandler.GetRewardParam(message));

            var content = workbook
                .UseTemplate(GetTemplateInfo(message.ActorLevel))
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData.RewardDetails, GetRowMapper(message.ActorLevel))
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), content);
        }

        private TemplateInfo GetTemplateInfo(UserLevel actorLevel)
            => new TemplateInfo(
                GetExcelTemplatePath($"Reward/RewardDetail{actorLevel.GetDirectDownLineLevel().DisplayName}"),
                Resources.NoInformationIsAvailable);

        private static string GetReportTitle(RewardDetailExcelQuery message)
            => $"{Resources.RewardDetail} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

        internal static Mapper<RewardDetailEntity> GetRowMapper(UserLevel actorLevel)
        {
            var mapper = new Mapper<RewardDetailEntity>()
                .Map(A, rp => rp.CreatedDate)
                .Map(B, rp => rp.ProgramName)
                .Map(C, rp => rp.MemberUsername)
                .Map(D, rp => rp.MemberAmount)
                .Map(E, rp => rp.MemberPaymentStatus ? Resources.Paid : Resources.Unpaid)
                .Map(F, rp =>
                {
                    if (rp.MemberPaymentDate.HasValue)
                    {
                        return rp.MemberPaymentDate.Value;
                    }

                    return string.Empty;
                })
                .Map(G, rp => rp.MemberFeedbackText);

            if (actorLevel.IsMasterOrSuperLevel())
            {
                mapper
                    .Map(H, rp => rp.IsDirectMember || string.IsNullOrWhiteSpace(rp.AgentUsername) ? string.Empty : rp.AgentUsername)
                    .Map(I, rp => rp.IsDirectMember || string.IsNullOrWhiteSpace(rp.AgentUsername) ? string.Empty : (rp.AgentPaymentStatus ? Resources.Paid : Resources.Unpaid))
                    .Map(J, rp =>
                    {
                        if (rp.IsDirectMember || string.IsNullOrWhiteSpace(rp.AgentUsername) || !rp.AgentPaymentDate.HasValue)
                        {
                            return string.Empty;
                        }

                        return rp.AgentPaymentDate.Value;
                    });
            }

            if (actorLevel.IsSuperLevel())
            {
                mapper
                    .Map(K, rp => rp.IsDirectMember ? string.Empty : rp.MasterUsername)
                    .Map(L, rp => rp.IsDirectMember ? string.Empty : (rp.MasterPaymentStatus ? Resources.Paid : Resources.Unpaid))
                    .Map(M, rp =>
                    {
                        if (!rp.IsDirectMember && rp.MasterPaymentDate.HasValue)
                        {
                            return rp.MasterPaymentDate.Value;
                        }

                        return string.Empty;
                    });
            }

            return mapper;
        }

        private string GetExcelFileName(RewardDetailExcelQuery message)
           => string.Join(
               "-",
               Resources.RewardDetail,
               message.CustName,
               message.FromDate.ToShortDateWithSlash(),
               message.ToDate.ToShortDateWithSlash());
    }
}