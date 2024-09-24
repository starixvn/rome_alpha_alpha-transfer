using System.Linq;
using System.Threading.Tasks;
using Alpha.Transfer.Extensions;
using Alpha.Transfer.Models.Reward;
using Alpha.Transfer.Params.Reward;
using Alpha.Transfer.Services;
using Alpha.Transfer.Site.Requests.Reward;
using Alpha.Transfer.Site.ViewModels.Reward;
using MediatR;

namespace Alpha.Transfer.Site.Handlers
{
    public class RewardHandler :
        IAsyncRequestHandler<RewardPaymentQuery, RewardPaymentViewModel>,
        IAsyncRequestHandler<GetRewardPaymentDataQuery, RewardPayment>,
        IAsyncRequestHandler<UpdateRewardPaymentStatusCommand, RewardResult>,
        IAsyncRequestHandler<RewardDetailQuery, RewardDetailViewModel>,
        IAsyncRequestHandler<GetRewardDetailDataQuery, RewardDetailDataViewModel>
    {
        private readonly RewardService rewardService;
        private readonly BelongingService belongingService;

        public RewardHandler(RewardService rewardService, BelongingService belongingService)
        {
            this.rewardService = rewardService;
            this.belongingService = belongingService;
        }

        #region Reward Payment
        public async Task<RewardPaymentViewModel> Handle(RewardPaymentQuery message)
        {
            var programsTask = rewardService.GetRewardPrograms();
            var reportDataTask = rewardService.GetRewardPayments(GetRewardParam(message));
            await Task.WhenAll(programsTask, reportDataTask);

            return new RewardPaymentViewModel(message, await programsTask, await reportDataTask);
        }

        public Task<RewardPayment> Handle(GetRewardPaymentDataQuery message)
            => rewardService.GetRewardPayments(GetRewardParam(message));

        public async Task<RewardResult> Handle(UpdateRewardPaymentStatusCommand message)
        {
            var custIds = string.Join("^", message.Data.Select(p => p.CustId));
            var isValidCheckMultipleCustomersBelonging = await belongingService.CheckMultipleCustomersBelonging(
                message.ViewerId, message.SubAccId, custIds);

            return isValidCheckMultipleCustomersBelonging
                ? await rewardService.UpdateRewardPaymentStatus(new UpdateRewardPaymentStatusParam(
                    message.Data,
                    message.ViewerId,
                    message.SubAccId,
                    message.IP,
                    message.LanguageKey))
                : new RewardResult(isKickout: true);
        }

        #endregion

        #region Reward Detail
        public async Task<RewardDetailViewModel> Handle(RewardDetailQuery message)
        {
            var programsTask = rewardService.GetRewardPrograms();
            var reportDataTask = rewardService.GetRewardDetails(GetRewardParam(message));
            await Task.WhenAll(programsTask, reportDataTask);

            return new RewardDetailViewModel(message, await programsTask, await reportDataTask);
        }

        public async Task<RewardDetailDataViewModel> Handle(GetRewardDetailDataQuery message)
        {
            var reportData = await rewardService.GetRewardDetails(GetRewardParam(message));

            return new RewardDetailDataViewModel(message.ActorLevel, reportData);
        }
        #endregion

        internal static RewardParam GetRewardParam(RewardBaseQuery message)
            => new RewardParam(
                message.ViewerId,
                message.ActorLevel,
                message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                message.ToDate,
                message.ProgramId,
                message.PaymentStatus,
                message.Username,
                message.PageIndex,
                message.PageSize);
    }
}