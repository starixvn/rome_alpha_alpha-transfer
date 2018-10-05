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
        private readonly TransferAllAndClearCreditService service;
        private readonly TransferAllAndClearCreditHandler handler;

        public TransferAllAndClearCreditHandlerTests()
        {
            user = Substitute.For<IUser>();
            service = Substitute.For<TransferAllAndClearCreditService>();
            handler = new TransferAllAndClearCreditHandler(service);
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
            service.GetCalculateResult(0, "username", string.Empty).Returns(calculationResult);

            var expectedViewModel = new CalculationViewModel(message, calculationResult);

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandlerTransferResultQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            user.LevelId.Returns((byte)3);
            var message = new TransferResultQuery(user)
            {
                TransferSteps = string.Empty,
                ClearCreditSteps = string.Empty
            };

            var transferResult = new TransferResult();
            service.GetTransferResult(Arg.Any<GetTransferResultParam>()).Returns(transferResult);

            var param = new GetTransferResultParam(0, 0, 0, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false);
            var expectedViewModel = new TransferResultViewModel(message, param, transferResult);

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }
    }
}
