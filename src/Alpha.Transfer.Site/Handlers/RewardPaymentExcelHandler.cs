using System.Threading.Tasks;
using Alpha.Transfer.AppResources;
using Alpha.Transfer.Extensions;
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
            var reportData = await rewardService.GetRewardPayments(RewardHandler.GetRewardParam(message));

            var content = workbook
                .UseTemplate(GetTemplateInfo())
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData.RewardPayments, GetRowMapper(false))
                .Footer(reportData.Total, GetRowMapper(true))
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), content);
        }

        private TemplateInfo GetTemplateInfo()
            => new TemplateInfo(
                GetExcelTemplatePath("Reward/RewardPayment"),
                Resources.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4);

        private static string GetReportTitle(RewardPaymentExcelQuery message)
            => $"{Resources.RewardPayment} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

        internal static Mapper<RewardPaymentEntity> GetRowMapper(bool isTotal)
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