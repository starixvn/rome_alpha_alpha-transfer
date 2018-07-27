namespace Alpha.Transfer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.TransferAllAndClearCredit;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Models.TransferAllAndClearCredit;
    using Params.TransferAllAndClearCredit;

    public interface TransferAllAndClearCreditService
    {
        IEnumerable<TransferStep> GetTransferSteps(string username);

        IEnumerable<ClearCreditStep> GetClearCreditSteps(string username);

        TransferResult GetTransferResult(GetTransferResultParam param);
    }

    public class TransferAllAndClearCreditServiceImpl : TransferAllAndClearCreditService
    {
        private readonly IDynamicRepository repository;
        private readonly TransferAllAndClearCreditDataAccess transferAllAndClearCreditDataAccess;

        public TransferAllAndClearCreditServiceImpl(
            IDynamicRepository dynamicRepository,
            TransferAllAndClearCreditDataAccess transferAllAndClearCreditDataAccess)
        {
            repository = dynamicRepository;
            this.transferAllAndClearCreditDataAccess = transferAllAndClearCreditDataAccess;
        }

        public IEnumerable<TransferStep> GetTransferSteps(string username)
        {
            var result = new List<TransferStep>
            {
                new TransferStep("Member to Agent", 10000, 5),
                new TransferStep("Agent to CHU01", 660, null),
                new TransferStep("CHU01 to your account", 330, null)
            };

            return result;

            // return repository.Fetch<TransferStep>(new GetTransferStepsCriteria(username));
        }

        public IEnumerable<ClearCreditStep> GetClearCreditSteps(string username)
        {
            var result = new List<ClearCreditStep>
            {
                new ClearCreditStep("CHU01's Members", 10000, 9995),
                new ClearCreditStep("CHU01's Agent", 20000, 1993),
                new ClearCreditStep("CHU01", 40000, 39991)
            };

            return result;

            // return repository.Fetch<ClearCreditStep>(new GetClearCreditStepsCriteria(username));
        }

        public TransferResult GetTransferResult(GetTransferResultParam param)
        {
            var criteria = new GetTransferResultCriteria(param.Username, param.IsClearCredit);
            var transferResult = transferAllAndClearCreditDataAccess.GetTransferResult(criteria);

            foreach (var step in transferResult.TransferSteps)
            {
                var oldStep = param.TransferSteps?.FirstOrDefault(s => string.Equals(s.Name, step.Name, StringComparison.OrdinalIgnoreCase));
                step.OldOutstanding = oldStep?.Outstanding;
            }

            foreach (var step in transferResult.ClearCreditSteps)
            {
                var oldStep = param.ClearCreditSteps?.FirstOrDefault(s => string.Equals(s.Level, step.Level, StringComparison.OrdinalIgnoreCase));
                step.OldClearableCredit = oldStep?.ClearableCredit ?? 0;
            }

            return transferResult;
        }
    }
}
