namespace Alpha.Transfer.Services
{
    using System.Threading.Tasks;
    using Alpha.Transfer.Criteria;
    using Alpha.Transfer.DataAccesses;

    public interface TransferService
    {
        bool IsAllDayBalanceTransfer(int custId);

        Task<bool> IsAllDayBalanceTransferAsync(int custId);
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
    }
}
