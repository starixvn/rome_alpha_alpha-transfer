using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Transfer.Extensions;
using Alpha.Transfer.Models;
using Alpha.Transfer.Models.Customer;
using Alpha.Transfer.Models.Reward;
using Alpha.Transfer.Params.Reward;
using Alpha.Transfer.Services;
using Alpha.Transfer.Site.Requests.Reward;
using Alpha.Transfer.Site.ViewModels.Reward;
using Fanex.Logging.Extensions.Web;
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
        private readonly CustomerService customerService;

        public RewardHandler(
            RewardService rewardService,
            BelongingService belongingService,
            CustomerService customerService)
        {
            this.rewardService = rewardService;
            this.belongingService = belongingService;
            this.customerService = customerService;
        }

        #region Reward Payment
        public async Task<RewardPaymentViewModel> Handle(RewardPaymentQuery message)
        {
            var programs = await rewardService.GetRewardPrograms();
            if (string.IsNullOrWhiteSpace(message.ProgramIds))
            {
                message.ProgramIds = string.Join(",", programs.Select(p => p.ResourceId));
            }

            var reportData = await rewardService.GetRewardPayments(GetRewardParam(message));

            return new RewardPaymentViewModel(message, programs, reportData);
        }

        public Task<RewardPayment> Handle(GetRewardPaymentDataQuery message)
        {
            if (!string.IsNullOrWhiteSpace(message.Username) && !message.IsInternal)
            {
               UpdateUserSearchInRewardPaymentTab(message.Username, message.ViewerId, message.ActorLevel);
            }

            return rewardService.GetRewardPayments(GetRewardParam(message));
        }

        private void UpdateUserSearchInRewardPaymentTab(string username, int viewerId, UserLevel actorLevel)
        {
            Task.Run(async () =>
            {
                try
                {
                    var custInfo = await customerService.GetCustomerLevel(username, viewerId);

                    if (custInfo != null && (actorLevel.IsDirectUplineOf(custInfo.RoleId) || IsDirectMember(actorLevel, custInfo)))
                    {
                        await customerService.UpdateUserSearch(viewerId, custInfo.Username, custInfo.FirstName, custInfo.LastName, custInfo.Username2);
                    }
                }
                catch (Exception ex)
                {
                    await ExceptionHandler.HandleAsync(ex);
                }
            });
        }

        private static bool IsDirectMember(UserLevel actorLevel, CustomerLevel custInfo)
            => custInfo.RoleId == UserLevel.Member.Value
            && custInfo.AgentId.HasValue
            && -custInfo.AgentId.Value == actorLevel.Value;

        public async Task<RewardResult> Handle(UpdateRewardPaymentStatusCommand message)
        {
            var isValidCheckMultipleCustomersBelonging = await CheckMultipleCustomersBelonging(message);

            return isValidCheckMultipleCustomersBelonging
                ? await rewardService.UpdateRewardPaymentStatus(new UpdateRewardPaymentStatusParam(
                    message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                    message.ToDate,
                    message.Data,
                    message.ViewerId,
                    message.SubAccId,
                    message.SubAccName,
                    message.IP,
                    message.LanguageKey,
                    message.AGIP))
                : new RewardResult(isKickout: true);
        }

        private async Task<bool> CheckMultipleCustomersBelonging(UpdateRewardPaymentStatusCommand message)
        {
            var normalCustIds = new List<int>();
            var directMemberCustIds = new List<int>();

            foreach (var item in message.Data)
            {
                if (item.IsDirectMember)
                {
                    directMemberCustIds.Add(item.CustId);
                }
                else
                {
                    normalCustIds.Add(item.CustId);
                }
            }

            var belongingNormalCustTask = normalCustIds.Count > 0
                ? belongingService.CheckMultipleCustomersBelonging(message.ViewerId, message.SubAccId, string.Join("^", normalCustIds))
                : Task.FromResult(true);
            var belongingDirectMemberTask = directMemberCustIds.Count > 0
                ? belongingService.CheckMultipleCustomersBelonging(message.ViewerId, message.SubAccId, string.Join("^", directMemberCustIds))
                : Task.FromResult(true);
            await Task.WhenAll(belongingNormalCustTask, belongingDirectMemberTask);

            return await belongingNormalCustTask && await belongingDirectMemberTask;
        }

        #endregion

        #region Reward Detail
        public async Task<RewardDetailViewModel> Handle(RewardDetailQuery message)
        {
            var programs = await rewardService.GetRewardPrograms();
            if (string.IsNullOrWhiteSpace(message.ProgramIds))
            {
                message.ProgramIds = string.Join(",", programs.Select(p => p.ResourceId));
            }

            var reportData = await rewardService.GetRewardDetails(GetRewardParam(message));

            return new RewardDetailViewModel(message, programs, reportData);
        }

        public async Task<RewardDetailDataViewModel> Handle(GetRewardDetailDataQuery message)
        {
            if (!string.IsNullOrWhiteSpace(message.Username) && !message.IsInternal)
            {
                UpdateUserSearchInRewardDetailTab(message.Username, message.ViewerId, message.ActorLevel);
            }

            var reportData = await rewardService.GetRewardDetails(GetRewardParam(message));

            return new RewardDetailDataViewModel(message.ActorLevel, reportData);
        }

        private void UpdateUserSearchInRewardDetailTab(string username, int viewerId, UserLevel actorLevel)
        {
            Task.Run(async () =>
            {
                try
                {
                    var custInfo = await customerService.GetCustomerLevel(username, viewerId);

                    if (custInfo != null)
                    {
                        var nickname = actorLevel.IsDirectUplineOf(custInfo.RoleId)
                            ? custInfo.Username2
                            : string.Empty;
                        await customerService.UpdateUserSearch(viewerId, custInfo.Username, custInfo.FirstName, custInfo.LastName, nickname);
                    }
                }
                catch (Exception ex)
                {
                    await ExceptionHandler.HandleAsync(ex);
                }
            });
        }
        #endregion

        internal static RewardParam GetRewardParam(RewardBaseQuery message)
            => new RewardParam(
                message.ViewerId,
                message.ActorLevel,
                message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                message.ToDate,
                message.ProgramIds,
                message.PaymentStatus,
                message.LevelType,
                message.Username,
                message.PageIndex,
                message.PageSize);
    }
}