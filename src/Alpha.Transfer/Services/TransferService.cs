namespace Alpha.Transfer.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Transfer.Criteria;
    using Alpha.Transfer.DataAccesses;
    using AlphaPlus.User.Models;

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
        private readonly TransferDataAccess transferDataAccess;

        public TransferServiceImpl(TransferDataAccess transferDataAccess)
            => this.transferDataAccess = transferDataAccess;

        public bool IsAllDayBalanceTransfer(int custId)
            => transferDataAccess.IsAllDayBalanceTransfer(new GetIsAllDayBalTransferCriteria(custId));

        public async Task<bool> IsAllDayBalanceTransferAsync(int custId)
            => await transferDataAccess.IsAllDayBalanceTransferAsync(new GetIsAllDayBalTransferCriteria(custId));

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
