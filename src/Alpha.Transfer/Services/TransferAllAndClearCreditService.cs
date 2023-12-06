namespace Alpha.Transfer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Criteria.TransferAllAndClearCredit;
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
        private readonly IDynamicRepository repository;

        public TransferAllAndClearCreditServiceImpl(IDynamicRepository dynamicRepository)
            => repository = dynamicRepository;

        public TransferResult GetCalculateResult(GetCalculationResultParam param)
        {
            var getDownlineCriteria = new GetDownlineTransferCriteria(param.CustId, param.Username);
            repository.Execute(getDownlineCriteria);

            var getNetPositionCriteria = new GetNetPositionCriteria(getDownlineCriteria.DownlineCustId, getDownlineCriteria.DownlineRoleId);
            var netPositionModels = repository.Fetch<NetPositionModel>(getNetPositionCriteria, param.AGIP);

            var criteria = new GetCalculateResultCriteria(
                param.CustId, 
                param.Username, 
                param.Language, 
                param.AllowedTotalBalanceTransferSuperIds, 
                param.IsAllDayBalanceTransfer,
                BuildDownlineNetPositionXml(netPositionModels),
                getDownlineCriteria.DownlineCustId);

            var (conditionSteps, transferSteps, clearCreditSteps)
                = repository.FetchMultiple<ConditionStep, TransferStep, ClearCreditStep>(criteria);

            return new TransferResult(transferSteps, clearCreditSteps, conditionSteps.Select(c => c.Content));
        }

        private static string BuildDownlineNetPositionXml(IEnumerable<NetPositionModel> netPositions)
        {
            var xmlData = new StringBuilder("<ROOT>");
            foreach (var item in netPositions)
            {
                xmlData.Append(item.ToXML);
            }
            xmlData.Append("</ROOT>");

            return xmlData.ToString();
        }

        public TransferResult GetTransferResult(GetTransferResultParam param)
        {
            var skipTransfer = param.SkipTransfer();
            TransferAllResult transferResult;

            if (skipTransfer)
            {
                transferResult = new TransferAllResult();
            }
            else
            {
                var criteria = new TransferAllCriteria(
                    param.CustId,
                    param.UserId,
                    param.SubAccId,
                    param.Username,
                    param.AllowedTotalBalanceTransferSuperIds,
                    param.IP,
                    param.Language,
                    param.IsAllDayBalanceTransfer);
                var (conditionSteps, transferSteps) = repository.FetchMultiple<ConditionStep, TransferStep>(criteria, param.AGIP);

                transferResult = new TransferAllResult(transferSteps, criteria.ErrCode, criteria.ErrMsg, conditionSteps.Select(c => c.Content));
            }

            var clearCreditResult = (skipTransfer || transferResult.IsSuccess) && param.IsClearCredit
                ? ClearCredit(param)
                : new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 1, string.Empty, Enumerable.Empty<string>());

            return new TransferResult(transferResult, clearCreditResult, skipTransfer);
        }

        private ClearCreditResult ClearCredit(GetTransferResultParam param)
        {
            var command = new GetCustomerInfoCriteria(param.Username);
            repository.Execute(command);

            var totalAmts = repository.Fetch<TotalAmtModel>(new GetTotalAmtCriteria(command.CustId, command.RoleId), param.AGIP);
            var criteria = new ClearCreditCriteria(
                param.CustId,
                param.UserId,
                param.SubAccId,
                param.Username,
                param.IP,
                param.Language,
                BuildMemberAmountXml(totalAmts));
            var (conditionSteps, clearCreditSteps) = repository.FetchMultiple<ConditionStep, ClearCreditStep>(criteria);

            return new ClearCreditResult(clearCreditSteps, criteria.ErrCode, criteria.ErrMsg, conditionSteps.Select(c => c.Content));
        }

        private static string BuildMemberAmountXml(IEnumerable<TotalAmtModel> totalAmts)
        {
            var xmlData = new StringBuilder("<ROOT>");
            foreach (var amt in totalAmts)
            {
                xmlData.Append(amt.ToXML);
            }
            xmlData.Append("</ROOT>");

            return xmlData.ToString();
        }
    }
}
