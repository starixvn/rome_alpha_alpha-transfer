namespace Alpha.Transfer.Site.Tests.ViewModels.TransferAllAndClearCredit
{
    using System.Collections.Generic;
    using NSubstitute;
    using Requests.TransferAllAndClearCredit;
    using Site.ViewModels.TransferAllAndClearCredit;
    using Transfer.Models;
    using Transfer.Models.TransferAllAndClearCredit;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    public class CalculationViewModelTests
    {
        private readonly IUser user;
        private readonly CalculationQuery query;
        private readonly CalculationViewModel viewModel;

        public CalculationViewModelTests()
        {
            user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);

            query = new CalculationQuery(user)
            {
                Username = "username"
            };

            var calculationResult = new TransferResult();

            viewModel = new CalculationViewModel(query, calculationResult);
        }

        [Fact]
        public void GetOutstandingText_NullOutstanding_ReturnNA()
        {
            // Arrange
            var expectedValue = "N/A";

            // Act
            var actualValue = viewModel.GetOutstandingText(null);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetOutstandingText_HasOutstanding_ReturnCorrectValue()
        {
            // Arrange
            var outstanding = 1000;
            var expectedValue = "1,000.00";

            // Act
            var actualValue = viewModel.GetOutstandingText(outstanding);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetTransferStepName_SuperLevelAndMemberStep_ReturnCorrectValue()
        {
            // Arrange
            var expectedValue = "Member to Agent";
            var step = new TransferStep(1, 2, 3);

            // Act
            var actualValue = viewModel.GetTransferStepName(step);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetTransferStepName_SuperLevelAndAgentStep_ReturnCorrectValue()
        {
            // Arrange
            var expectedValue = "Agent to username";
            var step = new TransferStep(2, 2, 3);

            // Act
            var actualValue = viewModel.GetTransferStepName(step);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetTransferStepName_SuperLevelAndMasterStep_ReturnCorrectValue()
        {
            // Arrange
            var expectedValue = "username to your account";
            var step = new TransferStep(3, 2, 3);

            // Act
            var actualValue = viewModel.GetTransferStepName(step);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetClearCreditStepName_SuperLevelAndMemberStep_ReturnCorrectValue()
        {
            // Arrange
            var expectedValue = "username's Members";
            var step = new ClearCreditStep(1, 2, 3);

            // Act
            var actualValue = viewModel.GetClearCreditStepName(step);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetClearCreditStepName_SuperLevelAndAgentStep_ReturnCorrectValue()
        {
            // Arrange
            var expectedValue = "username's Agents";
            var step = new ClearCreditStep(2, 2, 3);

            // Act
            var actualValue = viewModel.GetClearCreditStepName(step);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetClearCreditStepName_SuperLevelAndMasterStep_ReturnCorrectValue()
        {
            // Arrange
            var expectedValue = "username";
            var step = new ClearCreditStep(3, 2, 3);

            // Act
            var actualValue = viewModel.GetClearCreditStepName(step);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        public void DirectDownlineText_Always_ReturnCorrectValue(byte level, string expectedValue)
        {
            // Arrange
            var query = new CalculationQuery(user)
            {
                CustLevelId = level
            };

            var calculationResult = new TransferResult();

            var viewModel = new CalculationViewModel(query, calculationResult);

            // Act
            var actualValue = viewModel.DirectDownlineText;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void CanClearCredit_EmptyClearCreditSteps_ReturnFalse()
        {
            // Arrange
            var transferSteps = new List<TransferStep>();
            var clearCreditSteps = new List<ClearCreditStep>();
            var calculationResult = new TransferResult(transferSteps, clearCreditSteps);
            var viewModel = new CalculationViewModel(query, calculationResult);

            // Act
            var actualValue = viewModel.CanClearCredit();

            // Assert
            Assert.False(actualValue);
        }

        [Fact]
        public void CanClearCredit_HaveCleareableAndUnclearable_ReturnTrue()
        {
            // Arrange
            var transferSteps = new List<TransferStep>();
            var clearCreditSteps = new List<ClearCreditStep>
            {
                new ClearCreditStep(1, 1, 2),
                new ClearCreditStep(1, 1, 0)
            };

            var calculationResult = new TransferResult(transferSteps, clearCreditSteps);
            var viewModel = new CalculationViewModel(query, calculationResult);

            // Act
            var actualValue = viewModel.CanClearCredit();

            // Assert
            Assert.True(actualValue);
        }

        [Fact]
        public void CanClearCredit_HaveUnclearableAndNotHaveCleareable_ReturnTrue()
        {
            // Arrange
            var transferSteps = new List<TransferStep>();
            var clearCreditSteps = new List<ClearCreditStep>
            {
                new ClearCreditStep(1, 1, 0)
            };

            var calculationResult = new TransferResult(transferSteps, clearCreditSteps);
            var viewModel = new CalculationViewModel(query, calculationResult);

            // Act
            var actualValue = viewModel.CanClearCredit();

            // Assert
            Assert.False(actualValue);
        }

        [Fact]
        public void CanTransfer_NotCanClearCreditStepsAndEmptyTransferSteps_ReturnFalse()
        {
            // Arrange
            var transferSteps = new List<TransferStep>();
            var clearCreditSteps = new List<ClearCreditStep>();
            var calculationResult = new TransferResult(transferSteps, clearCreditSteps);
            var viewModel = new CalculationViewModel(query, calculationResult);

            // Act
            var actualValue = viewModel.CanTransfer();

            // Assert
            Assert.False(actualValue);
        }

        [Fact]
        public void CanTransfer_NotCanClearCreditStepsAndHaveTransfereableAndUntransferable_ReturnTrue()
        {
            // Arrange
            var transferSteps = new List<TransferStep>
            {
                new TransferStep(1, 1, 2),
                new TransferStep(1, 0, 3)
            };
            var clearCreditSteps = new List<ClearCreditStep>();

            var calculationResult = new TransferResult(transferSteps, clearCreditSteps);
            var viewModel = new CalculationViewModel(query, calculationResult);

            // Act
            var actualValue = viewModel.CanTransfer();

            // Assert
            Assert.True(actualValue);
        }

        [Fact]
        public void CanTransfer_NotCanClearCreditStepsAndHaveUntransferableAndNotHaveTransfereable_ReturnFalse()
        {
            // Arrange
            var transferSteps = new List<TransferStep>()
            {
                new TransferStep(1, 0, 2)
            };
            var clearCreditSteps = new List<ClearCreditStep>();

            var calculationResult = new TransferResult(transferSteps, clearCreditSteps);
            var viewModel = new CalculationViewModel(query, calculationResult);

            // Act
            var actualValue = viewModel.CanTransfer();

            // Assert
            Assert.False(actualValue);
        }

        [Fact]
        public void CanTransfer_CanClearCreditStepsAndEmptyTransferSteps_ReturnFalse()
        {
            // Arrange
            var transferSteps = new List<TransferStep>();
            var clearCreditSteps = new List<ClearCreditStep>
            {
                new ClearCreditStep(1, 1, 2),
                new ClearCreditStep(1, 1, 0)
            };
            var calculationResult = new TransferResult(transferSteps, clearCreditSteps);
            var viewModel = new CalculationViewModel(query, calculationResult);

            // Act
            var actualValue = viewModel.CanTransfer();

            // Assert
            Assert.False(actualValue);
        }

        [Fact]
        public void CanTransfer_CanClearCreditStepsAndHaveTransfereableAndUntransferable_ReturnTrue()
        {
            // Arrange
            var transferSteps = new List<TransferStep>
            {
                new TransferStep(1, 1, 2),
                new TransferStep(1, 0, 3)
            };
            var clearCreditSteps = new List<ClearCreditStep>
            {
                new ClearCreditStep(1, 1, 2),
                new ClearCreditStep(1, 1, 0)
            };

            var calculationResult = new TransferResult(transferSteps, clearCreditSteps);
            var viewModel = new CalculationViewModel(query, calculationResult);

            // Act
            var actualValue = viewModel.CanTransfer();

            // Assert
            Assert.True(actualValue);
        }

        [Fact]
        public void CanTransfer_CanClearCreditStepsAndHaveUntransferableAndNotHaveTransfereable_ReturnFalse()
        {
            // Arrange
            var transferSteps = new List<TransferStep>()
            {
                new TransferStep(1, 0, 3)
            };
            var clearCreditSteps = new List<ClearCreditStep>
            {
                new ClearCreditStep(1, 1, 2),
                new ClearCreditStep(1, 1, 0)
            };

            var calculationResult = new TransferResult(transferSteps, clearCreditSteps);
            var viewModel = new CalculationViewModel(query, calculationResult);

            // Act
            var actualValue = viewModel.CanTransfer();

            // Assert
            Assert.False(actualValue);
        }
    }
}
