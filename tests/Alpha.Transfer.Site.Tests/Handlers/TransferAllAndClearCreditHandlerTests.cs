namespace Alpha.Transfer.Site.Tests.Handlers
{
    using Extensions;
    using NSubstitute;
    using Params.TransferAllAndClearCredit;
    using Requests.TransferAllAndClearCredit;
    using Services;
    using Site.Handlers;
    using Site.ViewModels.TransferAllAndClearCredit;
    using Transfer.Models;
    using Transfer.Models.TransferAllAndClearCredit;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    public class TransferAllAndClearCreditHandlerTests
    {
        private readonly IUser user;
        private readonly TransferAllAndClearCreditService transferAllAndClearCreditService;
        private readonly TransferService transferService;
        private readonly TransferAllAndClearCreditHandler handler;

        public TransferAllAndClearCreditHandlerTests()
        {
            user = Substitute.For<IUser>();
            transferAllAndClearCreditService = Substitute.For<TransferAllAndClearCreditService>();
            transferService = Substitute.For<TransferService>();
            handler = new TransferAllAndClearCreditHandler(transferAllAndClearCreditService, transferService);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void HandlerCalculationQuery_EmptyUsername_ReturnsCorrectViewModel(string username)
        {
            // Arrange
            user.LevelId.Returns((byte)3);
            var message = new CalculationQuery(user);
            var expectedViewModel = new CalculationViewModel(message, new TransferResult());

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandlerCalculationQuery_HasUsername_ReturnsCorrectViewModel()
        {
            // Arrange
            user.LevelId.Returns((byte)3);
            var message = new CalculationQuery(user)
            {
                Username = "username"
            };

            var calculationResult = new TransferResult();
            transferAllAndClearCreditService.GetCalculateResult(Arg.Any<GetCalculationResultParam>()).Returns(calculationResult);

            var expectedViewModel = new CalculationViewModel(message, calculationResult);

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void HandlerTransferResultQuery_Always_ReturnsCorrectViewModel(bool isInternal)
        {
            // Arrange
            user.LevelId.Returns((byte)3);
            user.IsInternal.Returns(isInternal);
            var message = new TransferResultQuery(user)
            {
                TransferSteps = string.Empty,
                ClearCreditSteps = string.Empty
            };

            var transferResult = new TransferResult();
            transferAllAndClearCreditService.GetTransferResult(Arg.Any<GetTransferResultParam>()).Returns(transferResult);

            var param = new GetTransferResultParam(0, 0, 0, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false);
            var expectedViewModel = new TransferResultViewModel(message, param, transferResult);

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }
    }
}
