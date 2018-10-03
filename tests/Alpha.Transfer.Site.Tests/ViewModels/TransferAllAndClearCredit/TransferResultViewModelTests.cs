namespace Alpha.Transfer.Site.Tests.ViewModels.TransferAllAndClearCredit
{
    using System.Linq;
    using NSubstitute;
    using Params.TransferAllAndClearCredit;
    using Requests.TransferAllAndClearCredit;
    using Site.ViewModels.TransferAllAndClearCredit;
    using Transfer.Models;
    using Transfer.Models.TransferAllAndClearCredit;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    public class TransferResultViewModelTests
    {
        private readonly IUser user;
        private readonly TransferResultQuery query;
        private readonly GetTransferResultParam param;

        public TransferResultViewModelTests()
        {
            user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);

            query = new TransferResultQuery(user)
            {
                Username = "username"
            };

            param = new GetTransferResultParam(1, 2, 3, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, false)]
        public void IsTransferSuccess_Always_ReturnCorrectValue(int errorCode, bool expectedValue)
        {
            // Arrange
            var transferAllResult = new TransferAllResult(Enumerable.Empty<TransferStep>(), errorCode, string.Empty, Enumerable.Empty<string>());
            var clearCreditResult = new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 1, string.Empty, Enumerable.Empty<string>());
            var transferResult = new TransferResult(transferAllResult, clearCreditResult, false);
            var viewModel = new TransferResultViewModel(query, param, transferResult);

            // Act
            var actualValue = viewModel.IsTransferSuccess;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, false)]
        public void IsClearCreditSuccess_Always_ReturnCorrectValue(int errorCode, bool expectedValue)
        {
            // Arrange
            var transferAllResult = new TransferAllResult(Enumerable.Empty<TransferStep>(), 1, string.Empty, Enumerable.Empty<string>());
            var clearCreditResult = new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), errorCode, string.Empty, Enumerable.Empty<string>());
            var transferResult = new TransferResult(transferAllResult, clearCreditResult, false);
            var viewModel = new TransferResultViewModel(query, param, transferResult);

            // Act
            var actualValue = viewModel.IsClearCreditSuccess;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetOutstandingText_NullOutstanding_ReturnNA()
        {
            // Arrange
            var viewModel = new TransferResultViewModel(query, param, new TransferResult());
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
            var viewModel = new TransferResultViewModel(query, param, new TransferResult());
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
            var viewModel = new TransferResultViewModel(query, param, new TransferResult());
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
            var viewModel = new TransferResultViewModel(query, param, new TransferResult());
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
            var viewModel = new TransferResultViewModel(query, param, new TransferResult());
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
            var viewModel = new TransferResultViewModel(query, param, new TransferResult());
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
            var viewModel = new TransferResultViewModel(query, param, new TransferResult());
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
            var viewModel = new TransferResultViewModel(query, param, new TransferResult());
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
            var query = new TransferResultQuery(user)
            {
                CustLevelId = level
            };

            var transferAllResult = new TransferAllResult(Enumerable.Empty<TransferStep>(), 0, string.Empty, Enumerable.Empty<string>());
            var clearCreditResult = new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 1, string.Empty, Enumerable.Empty<string>());
            var transferResult = new TransferResult(transferAllResult, clearCreditResult, false);
            var viewModel = new TransferResultViewModel(query, param, transferResult);

            // Act
            var actualValue = viewModel.DirectDownlineText;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
