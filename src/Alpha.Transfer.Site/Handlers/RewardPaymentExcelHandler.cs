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
    public class RewardPaymentExcelHandler : ExcelBaseHandler, IAsyncRequestHandler<RewardPaymentExcelQuery, ExcelFileViewModel>
    {
        private readonly RewardService rewardService;
        private readonly Workbook<RewardPaymentEntity> workbook;

        public RewardPaymentExcelHandler(RewardService rewardService, Workbook<RewardPaymentEntity> workbook)
        {
            this.rewardService = rewardService;
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(RewardPaymentExcelQuery message)
        {
            var isMasterOrSuper = message.ActorLevel.IsMasterOrSuperLevel();
            var levelTypeValue = message.ActorLevel.IsSuperLevel() ? Resources.MasterAgent : Resources.Agent;
            var bodyRowMapper = isMasterOrSuper ? GetMasterAgentRowMapper(levelTypeValue, false) : GetMemberRowMapper(false);
            var totalRowMapper = isMasterOrSuper ? GetMasterAgentRowMapper(levelTypeValue, true) : GetMemberRowMapper(true);

            var reportData = await rewardService.GetRewardPayments(RewardHandler.GetRewardParam(message));
            var content = workbook
                .UseTemplate(GetTemplateInfo(message.ActorLevel))
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData.RewardPayments, bodyRowMapper)
                .Footer(reportData.Total, totalRowMapper)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), content);
        }

        private TemplateInfo GetTemplateInfo(UserLevel actorLevel)
            => new TemplateInfo(
                GetExcelTemplatePath($"Reward/RewardPayment{(actorLevel.IsMasterOrSuperLevel() ? string.Empty : "Member")}"),
                Resources.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4);

        private static string GetReportTitle(RewardPaymentExcelQuery message)
            => $"{Resources.RewardPayment} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

        internal static Mapper<RewardPaymentEntity> GetMasterAgentRowMapper(string levelTypeValue, bool isTotal)
        {
            var mapper = new Mapper<RewardPaymentEntity>();

            if (isTotal)
            {
                mapper
                    .Map(A, rp => Resources.Total)
                    .Map(D, rp => rp.Amount);
            }
            else
            {
                mapper
                    .Map(A, rp => rp.ProgramName)
                    .Map(B, rp => rp.Username)
                    .Map(C, rp => rp.IsDirectMember ? Resources.DirectMember : levelTypeValue)
                    .Map(D, rp => rp.Amount)
                    .Map(E, rp =>
                    {
                        if (rp.PaymentDate.HasValue)
                        {
                            return rp.PaymentDate.Value;
                        }

                        return string.Empty;
                    })
                    .Map(F, rp => rp.PaymentStatus ? Resources.Paid : Resources.Unpaid);
            }

            return mapper;
        }

        internal static Mapper<RewardPaymentEntity> GetMemberRowMapper(bool isTotal)
        {
            var mapper = new Mapper<RewardPaymentEntity>();

            if (isTotal)
            {
                mapper
                    .Map(A, rp => Resources.Total)
                    .Map(C, rp => rp.Amount);
            }
            else
            {
                mapper
                    .Map(A, rp => rp.ProgramName)
                    .Map(B, rp => rp.Username)
                    .Map(C, rp => rp.Amount)
                    .Map(D, rp =>
                    {
                        if (rp.PaymentDate.HasValue)
                        {
                            return rp.PaymentDate.Value;
                        }

                        return string.Empty;
                    })
                    .Map(E, rp => rp.PaymentStatus ? Resources.Paid : Resources.Unpaid);
            }

            return mapper;
        }

        private string GetExcelFileName(RewardPaymentExcelQuery message)
           => string.Join(
               "-",
               Resources.RewardPayment,
               message.CustName,
               message.FromDate.ToShortDateWithSlash(),
               message.ToDate.ToShortDateWithSlash());
    }
}