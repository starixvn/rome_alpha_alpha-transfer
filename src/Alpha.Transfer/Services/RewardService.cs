using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.Transfer.AppResources;
using Alpha.Transfer.Criteria.Reward;
using Alpha.Transfer.Models;
using Alpha.Transfer.Models.Reward;
using Alpha.Transfer.Params.Reward;
using Fanex.Data.Repository;
using Fanex.Logging.Extensions.Web;
using Newtonsoft.Json;

namespace Alpha.Transfer.Services
{
    public interface RewardService
    {
        Task<IEnumerable<RewardProgram>> GetRewardPrograms();

        Task<RewardPayment> GetRewardPayments(RewardParam param);

        Task<RewardResult> UpdateRewardPaymentStatus(UpdateRewardPaymentStatusParam param);

        Task<RewardDetail> GetRewardDetails(RewardParam param);
    }

    public class RewardServiceImpl : RewardService
    {
        private readonly IDynamicRepository repository;
        private readonly CacheRefService cacheRefService;

        public RewardServiceImpl(IDynamicRepository repository, CacheRefService cacheRefService)
            => (this.repository, this.cacheRefService) = (repository, cacheRefService);

        public async Task<IEnumerable<RewardProgram>> GetRewardPrograms()
        {
            var programs = await repository.FetchAsync<RewardProgram>(new GetRewardProgramsCriteria());

            return AssignProgramName(programs);
        }

        private IEnumerable<RewardProgram> AssignProgramName(IEnumerable<RewardProgram> programs)
        {
            foreach (var item in programs)
            {
                item.Name = cacheRefService.GetProgramName(item.ResourceId);
                yield return item;
            }
        }

        public async Task<RewardPayment> GetRewardPayments(RewardParam param)
        {
            var total = new RewardPaymentEntity();
            var rawData = await repository.FetchAsync<RewardPaymentEntity>(new GetRewardPaymentCriteria(
                param.CustId,
                param.CustLevel,
                param.FromDate,
                param.ToDate,
                param.ProgramId,
                param.PaymentStatus,
                param.Username));

            foreach (var item in rawData)
            {
                item.ProgramName = cacheRefService.GetProgramName(item.ProgramId);
                total += item;
            }

            return new RewardPayment(rawData, total);
        }

        public async Task<RewardResult> UpdateRewardPaymentStatus(UpdateRewardPaymentStatusParam param)
        {
            var errors = new List<string>();

            foreach (var item in param.Data)
            {
                var result = await UpdateRewardPaymentStatus(item, param);

                if (result.IsFailed)
                {
                    errors.Add($"{cacheRefService.GetProgramName(item.ProgramId)} - {item.Username}: {result.Message}");
                }
            }

            return new RewardResult(param.Data.Length, errors);
        }

        private async Task<Result> UpdateRewardPaymentStatus(RewardPaymentStatus data, UpdateRewardPaymentStatusParam param)
        {
            const int AlreadyPaidError = 101;
            const string SqlDateFormat = "yyyy-MM-dd";
            var criteria = new UpdateRewardPaymentStatusCriteria(
                JsonConvert.SerializeObject(new
                {
                    FromDate = param.FromDate.ToString(SqlDateFormat),
                    ToDate = param.ToDate.ToString(SqlDateFormat),
                    data.CustId,
                    data.ProgramId,
                    data.MaxTransId
                }, Formatting.None),
                param.ActorId,
                param.SubAccId,
                param.SubAccName,
                param.IP,
                param.Language);
            await repository.ExecuteAsync(criteria);
            var result = new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg };

            if (result.IsSuccessful)
            {
                await LogRewardPaymentStatus(criteria.PaymentLog);
            }
            else if (result.Code == AlreadyPaidError)
            {
                result.Message = Resources.AlreadyPaid;
            }

            return result;
        }
        
        private Task LogRewardPaymentStatus(string paymentLog)
        {
            try
            {
                return repository.ExecuteAsync(new RewardConfirmPaymentLogCriteria(paymentLog));
            }
            catch (Exception ex)
            {
                return ExceptionHandler.HandleAsync(ex);
            }
        }

        public async Task<RewardDetail> GetRewardDetails(RewardParam param)
        {
            var criteria = new GetRewardDetailCriteria(
                param.CustId,
                param.CustLevel,
                param.FromDate,
                param.ToDate,
                param.ProgramId,
                param.PaymentStatus,
                param.Username,
                param.PageIndex,
                param.PageSize);
            var rawData = await repository.FetchAsync<RewardDetailEntity>(criteria);

            foreach (var item in rawData)
            {
                item.ProgramName = cacheRefService.GetProgramName(item.ProgramId);
            }

            return new RewardDetail(rawData, param.PageIndex, param.PageSize, criteria.TotalRecords);
        }
    }
}
