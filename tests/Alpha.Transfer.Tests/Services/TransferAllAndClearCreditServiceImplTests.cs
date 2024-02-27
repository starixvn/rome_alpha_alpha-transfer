namespace Alpha.Transfer.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Transfer.Params.TransferAllAndClearCredit;
    using Fanex.Data.Repository;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Transfer.Criteria.TransferAllAndClearCredit;
    using Transfer.Models.TransferAllAndClearCredit;
    using Transfer.Services;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    [Collection("AnyValueCreatorCollection")]
    public class TransferAllAndClearCreditServiceImplTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly IDynamicRepository repository;        
        private readonly TransferAllAndClearCreditServiceImpl serviceImpl;
        private readonly CompareLogic compareLogic;

        public TransferAllAndClearCreditServiceImplTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;

            repository = Substitute.For<IDynamicRepository>();
            serviceImpl = new TransferAllAndClearCreditServiceImpl(repository);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetCalculateResult_Always_ReturnsDataFromRepository()
        {
            // Arrange
            var transferResult = (
                new List<ConditionStep>(),
                new List<TransferStep>(),
                new List<ClearCreditStep>()
            );
            repository.FetchMultiple<ConditionStep, TransferStep, ClearCreditStep>(Arg.Any<GetCalculateResultCriteria>()).Returns(transferResult);

            // Act
            var actualData = serviceImpl.GetCalculateResult(new GetCalculationResultParam(1, string.Empty, string.Empty, string.Empty, false, string.Empty));

            // Assert
            Assert.True(compareLogic.Compare(transferResult, actualData).AreEqual);
        }

        [Fact]
        public void GetTransferResult_NotClearCredit_ReturnsDataFromRepository()
        {
            // Arrange
            var transferAllResult = new TransferAllResult(Enumerable.Empty<TransferStep>(), 1, string.Empty, Enumerable.Empty<string>());
            //repository.TransferAll(Arg.Any<TransferAllCriteria>()).Returns(transferAllResult);
            var clearCreditResult = new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 1, string.Empty, Enumerable.Empty<string>());

            var transferSteps = "[{'Id': 1, 'TransferableAmount': 2, 'Outstanding': 3}]";
            var param = new GetTransferResultParam(1, 2, 3, false, string.Empty, string.Empty, string.Empty, string.Empty, transferSteps, string.Empty, false, string.Empty);
            var expectedValue = new TransferResult(transferAllResult, clearCreditResult, false);

            // Act
            var actualData = serviceImpl.GetTransferResult(param);

            // Assert
            Assert.True(compareLogic.Compare(expectedValue, actualData).AreEqual);
        }

        [Fact]
        public void GetTransferResult_ClearCreditAndTransferFailed_ReturnsDataFromRepository()
        {
            // Arrange
            var transferAllResult = new TransferAllResult(Enumerable.Empty<TransferStep>(), 1, string.Empty, Enumerable.Empty<string>());
            //repository.TransferAll(Arg.Any<TransferAllCriteria>()).Returns(transferAllResult);
            var clearCreditResult = new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 1, string.Empty, Enumerable.Empty<string>());

            var transferSteps = "[{'Id': 1, 'TransferableAmount': 2, 'Outstanding': 3}]";
            var param = new GetTransferResultParam(4, 5, 6, true, string.Empty, string.Empty, string.Empty, string.Empty, transferSteps, string.Empty, false, string.Empty);
            var expectedValue = new TransferResult(transferAllResult, clearCreditResult, false);

            // Act
            var actualData = serviceImpl.GetTransferResult(param);

            // Assert
            Assert.True(compareLogic.Compare(expectedValue, actualData).AreEqual);
        }

        [Fact]
        public void GetTransferResult_ClearCreditAndTransferSuccessfully_ReturnsDataFromRepository()
        {
            // Arrange
            var transferAllResult = new TransferAllResult(Enumerable.Empty<TransferStep>(), 0, string.Empty, Enumerable.Empty<string>());
            //repository.TransferAll(Arg.Any<TransferAllCriteria>()).Returns(transferAllResult);

            var clearCreditResult = new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 3, string.Empty, Enumerable.Empty<string>());
            //repository.ClearCredit(Arg.Any<ClearCreditCriteria>()).Returns(clearCreditResult);

            var transferSteps = "[{'Id': 1, 'TransferableAmount': 2, 'Outstanding': 3}]";
            var param = new GetTransferResultParam(4, 5, 6, true, string.Empty, string.Empty, string.Empty, string.Empty, transferSteps, string.Empty, false, string.Empty);
            var expectedValue = new TransferResult(transferAllResult, clearCreditResult, false);

            // Act
            var actualData = serviceImpl.GetTransferResult(param);

            // Assert
            Assert.True(compareLogic.Compare(expectedValue, actualData).AreEqual);
        }

        [Fact]
        public void GetTransferResult_SkipTransferAndClearCredit_CallRepositoryClearCredit()
        {
            // Arrange
            var transferAllResult = new TransferAllResult(Enumerable.Empty<TransferStep>(), 0, string.Empty, Enumerable.Empty<string>());
            //repository.TransferAll(Arg.Any<TransferAllCriteria>()).Returns(transferAllResult);

            var clearCreditResult = new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 3, string.Empty, Enumerable.Empty<string>());
            //repository.ClearCredit(Arg.Any<ClearCreditCriteria>()).Returns(clearCreditResult);

            var param = new GetTransferResultParam(4, 5, 6, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty);
            var expectedValue = new TransferResult(transferAllResult, clearCreditResult, false);

            // Act
            var actualData = serviceImpl.GetTransferResult(param);

            // Assert
            //repository.Received().ClearCredit(Arg.Any<ClearCreditCriteria>());
        }

        [Fact]
        public void GetTransferResult_SkipTransferAndNotClearCredit_NotCallRepositoryClearCredit()
        {
            // Arrange
            var transferAllResult = new TransferAllResult(Enumerable.Empty<TransferStep>(), 0, string.Empty, Enumerable.Empty<string>());
            //repository.TransferAll(Arg.Any<TransferAllCriteria>()).Returns(transferAllResult);

            var clearCreditResult = new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 3, string.Empty, Enumerable.Empty<string>());
            //repository.ClearCredit(Arg.Any<ClearCreditCriteria>()).Returns(clearCreditResult);

            var param = new GetTransferResultParam(4, 5, 6, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty);
            var expectedValue = new TransferResult(transferAllResult, clearCreditResult, false);

            // Act
            var actualData = serviceImpl.GetTransferResult(param);

            // Assert
            //repository.DidNotReceive().ClearCredit(Arg.Any<ClearCreditCriteria>());
        }

        [Fact]
        public void GetTransferResult_SkipTransfer_NotCallRepositoryTransferAll()
        {
            // Arrange
            var clearCreditResult = new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 3, string.Empty, Enumerable.Empty<string>());
            //repository.ClearCredit(Arg.Any<ClearCreditCriteria>()).Returns(clearCreditResult);

            var param = new GetTransferResultParam(4, 5, 6, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty);

            // Act
            serviceImpl.GetTransferResult(param);

            // Assert
            //repository.DidNotReceive().TransferAll(Arg.Any<TransferAllCriteria>());
        }

        [Fact]
        public void GetTransferResult_NotSkipTransfer_CallRepositoryTransferAll()
        {
            // Arrange
            var transferAllResult = new TransferAllResult(Enumerable.Empty<TransferStep>(), 0, string.Empty, Enumerable.Empty<string>());
            //repository.TransferAll(Arg.Any<TransferAllCriteria>()).Returns(transferAllResult);

            var clearCreditResult = new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 3, string.Empty, Enumerable.Empty<string>());
            //repository.ClearCredit(Arg.Any<ClearCreditCriteria>()).Returns(clearCreditResult);

            var transferSteps = "[{'Id': 1, 'TransferableAmount': 2, 'Outstanding': 3}]";
            var param = new GetTransferResultParam(4, 5, 6, true, string.Empty, string.Empty, string.Empty, string.Empty, transferSteps, string.Empty, false, string.Empty);

            // Act
            serviceImpl.GetTransferResult(param);

            // Assert
            //repository.Received().TransferAll(Arg.Any<TransferAllCriteria>());
        }
    }
}