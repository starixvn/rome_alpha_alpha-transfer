namespace Alpha.Transfer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Transfer.Configurations;
    using AppResources;
    using Criteria;
    using Criteria.CashBalanceTransfer;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using Models.CashBalanceTransfer;

    public interface CashBalanceTransferService
    {
        Task<CashBalanceTransfer> GetCashBalanceTransferAsync(int custId, bool isDirectMember, string agIP);

        Task<Result> AddCashBalanceTransferAsync(
            int fromCustId,
            int toCustId,
            decimal amount,
            int creator,
            string creatorName,
            int subAccId,
            string ip,
            string languageKey,
            string agIP,
            bool isDirectMember);

        Task<string> GetAllowTransfer(int custId);

        Task<Tuple<bool, IEnumerable<string>>> GetTransferCondition(int custId);

        Task<string> GetTransferOption(int custId);
    }

    public class CashBalanceTransferServiceImpl : CashBalanceTransferService
    {
        private const string AgentTransferMemberListCache = "AgentMultipleTransfer.{0}.{1}";
        private const string ConcurrentSingleTransferResultMessage = "<span style=\"color: red\">The transfer has been proceeding by {0}<br/>";

        private readonly IDynamicRepository repository;
        private readonly ICacheService cacheService;

        public CashBalanceTransferServiceImpl(
            IDynamicRepository dynamicRepository,
            ICacheService cacheService)
        {
            repository = dynamicRepository;
            this.cacheService = cacheService;
        }

        public async Task<CashBalanceTransfer> GetCashBalanceTransferAsync(int custId, bool isDirectMember, string agIP)
        {
            ICriteria criteria = new GetCashBalanceTransferCriteria(custId, isDirectMember);

            return await repository.GetAsync<CashBalanceTransfer>(criteria, agIP);
        }

        public async Task<Result> AddCashBalanceTransferAsync(
            int fromCustId,
            int toCustId,
            decimal amount,
            int creator,
            string creatorName,
            int subAccId,
            string ip,
            string languageKey,
            string agIP,
            bool isDirectMember)
        {
            string cacheKey = string.Format(AgentTransferMemberListCache, AppSettings.SiteLabel, creator);
            TransferCacheService transferCacheService = new TransferCacheServiceImpl(creator, creatorName, cacheService);

            var custIdCache = transferCacheService.Get(cacheKey) ?? new List<TransferCacheModel>();
            var concurrentTrasfer = transferCacheService.CacheContains(custIdCache, toCustId);

            if (concurrentTrasfer != null)
            {
                return new Result
                {
                    Code = 105,
                    Message = string.Format(ConcurrentSingleTransferResultMessage, concurrentTrasfer.AdminName)
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
                languageKey,
                isDirectMember);

            try
            {
                await repository.ExecuteAsync(criteria, agIP);
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
            var (isDaily, days) = await GetTransferCondition(custId);
            var transferCondition = isDaily ? Resources.Daily : string.Join(" - ", days);

            return Resources.AllowTransfer + (string.IsNullOrEmpty(transferCondition) ? string.Empty : " " + transferCondition);
        }

        public async Task<Tuple<bool, IEnumerable<string>>> GetTransferCondition(int custId)
        {
            string transferOption = await GetTransferOption(custId);

            char[] charArray = transferOption.ToCharArray();

            if (charArray.Length == 0)
            {
                return new Tuple<bool, IEnumerable<string>>(false, Enumerable.Empty<string>());
            }

            const int dayInWeek = 7;
            if (charArray[dayInWeek] != '1')
            {
                return new Tuple<bool, IEnumerable<string>>(true, Enumerable.Empty<string>());
            }

            var dayBuilder = new List<string>();

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

            for (int i = 1, max = charArray.Length; i < max; i++)
            {
                var id = i % dayInWeek;

                if (charArray[id] == '1')
                {
                    dayBuilder.Add(dayArray[id]);
                }
            }

            return new Tuple<bool, IEnumerable<string>>(false, dayBuilder);
        }

        public async Task<string> GetTransferOption(int custId)
        {
            var criteria = new GetTransferOptionCriteria(custId);

            await repository.ExecuteAsync(criteria);

            return criteria.TransferOption;
        }
    }
}