namespace Alpha.Transfer.Services
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using AppResources;
    using Criteria;
    using Criteria.CashBalanceTransfer;
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
    }

    public class CashBalanceTransferServiceImpl : CashBalanceTransferService
    {
        private readonly IDynamicRepository repository;

        public CashBalanceTransferServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
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
            string cacheKey = string.Format(TransferConstants.AgentTransferMemberListCache, creator);
            TransferCacheService cacheService = new TransferCacheServiceImplement(creator, creatorName);
            List<TransferCacheModel> custIdCache = cacheService.Get(cacheKey) ?? new List<TransferCacheModel>();
            var concurrentTrasfer = cacheService.CacheContains(custIdCache, toCustId);
            if (concurrentTrasfer != null)
            {
                return new Result
                {
                    Code = 105,
                    Message = string.Format(TransferConstants.ConcurrentSingleTransferResultMessage, concurrentTrasfer.AdminName)
                };
            }

            cacheService.CacheAdd(custIdCache, toCustId, creator, creatorName);
            cacheService.Set(cacheKey, custIdCache);

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
                cacheService.CacheRemoveSuccessTransferCustIds(cacheKey, new List<int> { toCustId });
            }

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? Resources.TransferSuccessfully : criteria.ErrMsg
            };
        }

        public async Task<string> GetAllowTransfer(int custId)
        {
            string transferOption = await GetTransferOption(custId);

            string transferTime = Resources.AllowTransfer;
            char[] charArray = transferOption.ToCharArray();

            if (charArray.Length > 0)
            {
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
                    day = day.Substring(3);
                }

                transferTime += charArray[7] == '1' ?
                    " " + day :
                    " " + Resources.Daily;
            }

            return transferTime;
        }

        private async Task<string> GetTransferOption(int custId)
        {
            var criteria = new GetTransferOptionCriteria(custId);

            await repository.ExecuteAsync(criteria);

            return criteria.TransferOption;
        }
    }
}
