namespace Alpha.Transfer.Services
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Alpha.Transfer.Configurations;
    using AppResources;
    using Criteria;
    using Criteria.CashBalanceTransfer;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using Models.CashBalanceTransfer;
    using Sunplus.Agent.Model.Transfer.Constants;
    using Sunplus.Agent.Model.Transfer.Extensions;
    using Sunplus.Agent.Model.Transfer.Models;

    public interface CashBalanceTransferService
    {
        Task<CashBalanceTransfer> GetCashBalanceTransferAsync(int custId);

        Task<Result> AddCashBalanceTransferAsync(
            int fromCustId,
            int toCustId,
            decimal amount,
            int creator,
            string creatorName,
            int subAccId,
            string ip,
            string languageKey);

        Task<string> GetAllowTransfer(int custId);
        Task<string> GetTransferCondition(int custId);
    }

    public class CashBalanceTransferServiceImpl : CashBalanceTransferService
    {
        private readonly IDynamicRepository repository;
        private readonly ICacheService cacheService;

        public CashBalanceTransferServiceImpl(
            IDynamicRepository dynamicRepository,
            ICacheService cacheService)
        {
            repository = dynamicRepository;
            this.cacheService = cacheService;
        }

        public async Task<CashBalanceTransfer> GetCashBalanceTransferAsync(int custId)
        {
            ICriteria criteria = new GetCashBalanceTransferCriteria(custId);

            return await repository.GetAsync<CashBalanceTransfer>(criteria);
        }

        public async Task<Result> AddCashBalanceTransferAsync(
            int fromCustId,
            int toCustId,
            decimal amount,
            int creator,
            string creatorName,
            int subAccId,
            string ip,
            string languageKey)
        {
            string cacheKey = string.Format(TransferConstants.AgentTransferMemberListCache, Web.Configurations.AppSettings.SiteLabel, creator);
            TransferCacheService transferCacheService = new TransferCacheServiceImplement(creator, creatorName, cacheService);

            var custIdCache = transferCacheService.Get(cacheKey) ?? new List<TransferCacheModel>();
            var concurrentTrasfer = transferCacheService.CacheContains(custIdCache, toCustId);

            if (concurrentTrasfer != null)
            {
                return new Result
                {
                    Code = 105,
                    Message = string.Format(TransferConstants.ConcurrentSingleTransferResultMessage, concurrentTrasfer.AdminName)
                };
            }

            transferCacheService.CacheAdd(custIdCache, toCustId, creator, creatorName);
            transferCacheService.Set(cacheKey, custIdCache);

            var criteria = new AddCashBalanceTransferCriteria(
                fromCustId,
                toCustId,
                amount * -1,
                creator,
                subAccId,
                ip,
                languageKey);

            try
            {
                await repository.ExecuteAsync(criteria);
            }
            finally
            {
                transferCacheService.CacheRemoveSuccessTransferCustIds(cacheKey, new List<int> { toCustId });
            }

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? Resources.TransferSuccessfully : criteria.ErrMsg
            };
        }

        public async Task<string> GetAllowTransfer(int custId)
        {
            string transferCondition = await GetTransferCondition(custId);

            return Resources.AllowTransfer + (string.IsNullOrEmpty(transferCondition) ? string.Empty : " " + transferCondition);
        }

        public async Task<string> GetTransferCondition(int custId)
        {
            string transferOption = await GetTransferOption(custId);

            char[] charArray = transferOption.ToCharArray();

            if (charArray.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder dayBuilder = new StringBuilder();
            string[] dayArray =
            {
                Resources.Sunday,
                Resources.Monday,
                Resources.Tuesday,
                Resources.Wednesday,
                Resources.Thursday,
                Resources.Friday,
                Resources.Saturday
            };

            for (int i = 1, max = charArray.Length; i < max - 1; i++)
            {
                if (charArray[i] == '1')
                {
                    dayBuilder.Append(" - ").Append(dayArray[i]);
                }
            }

            if (charArray[0] == '1')
            {
                dayBuilder.Append(" - ").Append(dayArray[0]);
            }

            string day = dayBuilder.ToString();

            if (day.Length > 0)
            {
                const int strDayNameLength = 3;
                day = day.Substring(strDayNameLength);
            }

            const int dayInWeek = 7;

            return charArray[dayInWeek] == '1' ? day : Resources.Daily;
        }

        private async Task<string> GetTransferOption(int custId)
        {
            var criteria = new GetTransferOptionCriteria(custId);

            await repository.ExecuteAsync(criteria);

            return criteria.TransferOption;
        }
    }
}
