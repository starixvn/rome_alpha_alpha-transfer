namespace Alpha.Transfer.Services
{
    using System.Linq;
    using Criteria.TransferAllAndClearCredit;
    using DataAccesses;
    using Models.TransferAllAndClearCredit;
    using Params.TransferAllAndClearCredit;

    public interface TransferAllAndClearCreditService
    {
        TransferResult GetCalculateResult(int custId, string username);

        TransferResult GetTransferResult(GetTransferResultParam param);
    }

    public class TransferAllAndClearCreditServiceImpl : TransferAllAndClearCreditService
    {
        private readonly TransferAllAndClearCreditDataAccess transferAllAndClearCreditDataAccess;

        public TransferAllAndClearCreditServiceImpl(TransferAllAndClearCreditDataAccess transferAllAndClearCreditDataAccess)
        {
            this.transferAllAndClearCreditDataAccess = transferAllAndClearCreditDataAccess;
        }

        public TransferResult GetCalculateResult(int custId, string username)
        {
            var criteria = new GetCalculateResultCriteria(custId, username);
            var transferResult = transferAllAndClearCreditDataAccess.GetCalculateResult(criteria);
            return transferResult;
        }

        public TransferResult GetTransferResult(GetTransferResultParam param)
        {
            var skipTransfer = param.SkipTransfer();

            var transferResult = skipTransfer ?
                new TransferAllResult(Enumerable.Empty<TransferStep>(), 0, string.Empty) :
                transferAllAndClearCreditDataAccess.TransferAll(new TransferAllCriteria(
                    param.CustId,
                    param.UserId,
                    param.SubAccId,
                    param.Username,
                    param.AllowedTotalBalanceTransferSuperIds,
                    param.IP,
                    param.Language,
                    param.IsAllDayBalanceTransfer));

            var clearCreditResult = (skipTransfer || transferResult.IsSuccess) && param.IsClearCredit
                ? transferAllAndClearCreditDataAccess.ClearCredit(new ClearCreditCriteria(
                    param.CustId,
                    param.UserId,
                    param.SubAccId,
                    param.Username,
                    param.IP,
                    param.Language))
                    : new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 1, string.Empty);

            return new TransferResult(transferResult, clearCreditResult, skipTransfer);
        }
    }
}
