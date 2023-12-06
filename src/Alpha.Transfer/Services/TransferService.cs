namespace Alpha.Transfer.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Transfer.Criteria;
    using Alpha.Transfer.Models;
    using AlphaPlus.User.Models;
    using Fanex.Data.Repository;

    public interface TransferService
    {
        bool IsAllDayBalanceTransfer(int custId);

        Task<bool> IsAllDayBalanceTransferAsync(int custId);

        bool IsEnableAutoTransfer(
             int custId,
             string currency,
             bool isSubAcc,
             PageAccess subPageAccess,
             string offerCurrencies);
    }

    public class TransferServiceImpl : TransferService
    {
        private readonly IDynamicRepository dynamicRepository;

        public TransferServiceImpl(IDynamicRepository dynamicRepository)
            => this.dynamicRepository = dynamicRepository;

        public bool IsAllDayBalanceTransfer(int custId)
        {
            var transferModel = dynamicRepository.Get<TransferModel>(
                new GetIsAllDayBalTransferCriteria(custId));

            return transferModel != null && transferModel.IsAllDayBalTransfer;
        }

        public async Task<bool> IsAllDayBalanceTransferAsync(int custId)
        {
            var transferModel = await dynamicRepository.GetAsync<TransferModel>(
                new GetIsAllDayBalTransferCriteria(custId));

            return transferModel != null && transferModel.IsAllDayBalTransfer;
        }

        public bool IsEnableAutoTransfer(
            int custId,
            string currency,
            bool isSubAcc,
            PageAccess subPageAccess,
            string offerCurrencies)
        {
            if (isSubAcc && !subPageAccess.Transfer)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(offerCurrencies))
            {
                return false;
            }

            if (offerCurrencies == "*")
            {
                return true;
            }

            return offerCurrencies.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Any(c => string.Equals(currency, c.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}
