namespace Alpha.Transfer.Services
{
    using System.Linq;
    using Criteria.TransferAllAndClearCredit;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Models.TransferAllAndClearCredit;
    using Params.TransferAllAndClearCredit;

    public interface TransferAllAndClearCreditService
    {
        TransferResult GetCalculateResult(GetCalculationResultParam param);

        TransferResult GetTransferResult(GetTransferResultParam param);
    }

    public class TransferAllAndClearCreditServiceImpl : TransferAllAndClearCreditService
    {
        private readonly TransferAllAndClearCreditDataAccess transferAllAndClearCreditDataAccess;
        private readonly IDynamicRepository repository;

        public TransferAllAndClearCreditServiceImpl(
            IDynamicRepository dynamicRepository,
            TransferAllAndClearCreditDataAccess transferAllAndClearCreditDataAccess)
        {
            repository = dynamicRepository;
            this.transferAllAndClearCreditDataAccess = transferAllAndClearCreditDataAccess;
        }

        public TransferResult GetCalculateResult(GetCalculationResultParam param)
        {
            var getDownlineCriteria = new GetDownlineTransferCriteria(param.CustId, param.Username);
            repository.Execute(getDownlineCriteria);

            var getNetPositionCriteria = new GetNetPositionCriteria(getDownlineCriteria.DownlineCustId, getDownlineCriteria.DownlineRoleId);
            var netPositionModels = repository.Fetch<NetPositionModel>(getNetPositionCriteria);

            var criteria = new GetCalculateResultCriteria(
                param.CustId, 
                param.Username, 
                param.Language, 
                param.AllowedTotalBalanceTransferSuperIds, 
                param.IsAllDayBalanceTransfer,
                netPositionModels,
                getDownlineCriteria.DownlineCustId);

            var transferResult = transferAllAndClearCreditDataAccess.GetCalculateResult(criteria);
            return transferResult;
        }

        public TransferResult GetTransferResult(GetTransferResultParam param)
        {
            var skipTransfer = param.SkipTransfer();

            var transferResult = skipTransfer ?
                new TransferAllResult() :
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
                ? ClearCredit(param)
                : new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 1, string.Empty, Enumerable.Empty<string>());

            return new TransferResult(transferResult, clearCreditResult, skipTransfer);
        }

        private ClearCreditResult ClearCredit(GetTransferResultParam param)
        {
            var command = new GetCustomerInfoCriteria(param.Username);
            repository.Execute(command);

            var totalAmts = repository.Fetch<TotalAmtModel>(new GetTotalAmtCriteria(command.CustId, command.RoleId));

            var result = transferAllAndClearCreditDataAccess.ClearCredit(new ClearCreditCriteria(
                    param.CustId,
                    param.UserId,
                    param.SubAccId,
                    param.Username,
                    param.IP,
                    param.Language,
                    totalAmts));

            return result;
        }
    }
}
