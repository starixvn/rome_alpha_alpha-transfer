namespace Alpha.Transfer.Site.Tests.Handlers
{
    using System.Threading.Tasks;
    using AppResources;
    using Extensions;
    using NSubstitute;
    using Requests.CashBalanceTransfer;
    using Services;
    using Site.Handlers;
    using Site.ViewModels.CashBalanceTransfer;
    using Transfer.Models;
    using Transfer.Models.CashBalanceTransfer;
    using Xunit;

    [Trait("Category", "CashBalanceTransfer")]
    public class CashBalanceTransferHandlerTests
    {
        private readonly IUser user;
        private readonly CashBalanceTransferService cashBalanceTransferService;
        private readonly TransferService transferService;
        private readonly CashBalanceTransferHandler handler;

        public CashBalanceTransferHandlerTests()
        {
            user = Substitute.For<IUser>();
            user.Id.Returns(1);
            cashBalanceTransferService = Substitute.For<CashBalanceTransferService>();
            transferService = Substitute.For<TransferService>();
            handler = new CashBalanceTransferHandler(cashBalanceTransferService, transferService);
        }

        [Fact]
        public async Task HandlerCashBalanceTransferQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            int custId = 1176083;
            var message = new CashBalanceTransferQuery(user) { CustId = custId };
            var cashBalanceTransfer = new CashBalanceTransfer(
                1176083,
                3,
                "master1",
                67599433.9247M,
                1101240.3283M,
                342552.00M,
                900000002.00M,
                0);

            var expectedViewModel = new CashBalanceTransferViewModel(
                cashBalanceTransfer.UplineCashBalance,
                cashBalanceTransfer.CustId,
                cashBalanceTransfer.UserName,
                Enumeration.FromValue<UserLevel>(cashBalanceTransfer.RoleId),
                cashBalanceTransfer.CashBalance,
                cashBalanceTransfer.Outstanding,
                cashBalanceTransfer.GivenCredit,
                cashBalanceTransfer.PendingTransfer3rd);

            cashBalanceTransferService.GetCashBalanceTransferAsync(custId).Returns(cashBalanceTransfer);
            transferService.IsAllDayBalanceTransfer(Arg.Any<int>()).Returns(true);

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public async Task HandlerCashBalanceTransferQuery_DisableTransfer_HintFromResource()
        {
            // Arrange
            int custId = 1176083;
            var message = new CashBalanceTransferQuery(user) { CustId = custId };
            var cashBalanceTransfer = new CashBalanceTransfer(
                1176083,
                2,
                "agent1",
                0,
                1101240.3283M,
                1101240.3283M,
                900000002.00M,
                0);

            var hintFromResource = Resources.HintNotAvailableToTransfer;

            cashBalanceTransferService.GetCashBalanceTransferAsync(custId).Returns(cashBalanceTransfer);
            transferService.IsAllDayBalanceTransfer(Arg.Any<int>()).Returns(true);

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(hintFromResource, actualViewModel.Hint);
        }

        [Fact]
        public async Task HandlerCashBalanceTransferQuery_UserIsMember_CorrectCustLevelName()
        {
            // Arrange
            int custId = 1176083;
            var message = new CashBalanceTransferQuery(user) { CustId = custId };
            var cashBalanceTransfer = new CashBalanceTransfer(
                1176083,
                1,
                "member1",
                0,
                1101240.3283M,
                1101240.3283M,
                900000002.00M,
                0);

            var levelName = Resources.Member;

            cashBalanceTransferService.GetCashBalanceTransferAsync(custId).Returns(cashBalanceTransfer);
            transferService.IsAllDayBalanceTransfer(Arg.Any<int>()).Returns(true);

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(levelName, actualViewModel.CustLevelName);
        }

        [Fact]
        public async Task HandlerCashBalanceTransferQuery_UserIsAgent_CorrectCustLevelName()
        {
            // Arrange
            int custId = 1176083;
            var message = new CashBalanceTransferQuery(user) { CustId = custId };
            var cashBalanceTransfer = new CashBalanceTransfer(
                1176083,
                2,
                "agent1",
                0,
                1101240.3283M,
                1101240.3283M,
                900000002.00M,
                0);

            var levelName = Resources.Agent;

            cashBalanceTransferService.GetCashBalanceTransferAsync(custId).Returns(cashBalanceTransfer);
            transferService.IsAllDayBalanceTransfer(Arg.Any<int>()).Returns(true);

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(levelName, actualViewModel.CustLevelName);
        }

        [Fact]
        public async Task HandlerCashBalanceTransferCommand_Always_ReturnsSuccessfulResult()
        {
            // Arrange
            var expectedResult = new Result { Message = Resources.TransferSuccessfully };

            user.Id.Returns(1176070);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");

            var command = new CashBalanceTransferCommand(user)
            {
                CustId = 1176083,
                Amount = 1101240.32M
            };

            cashBalanceTransferService.AddCashBalanceTransferAsync(
                command.UplineId,
                command.CustId,
                command.Amount,
                command.UplineId,
                command.UplineName,
                command.SubAccId,
                command.IP,
                command.LanguageKey).Returns(expectedResult);
            transferService.IsAllDayBalanceTransfer(Arg.Any<int>()).Returns(true);

            // Act
            var actualResult = await handler.Handle(command);

            // Assert
            AssertExtension.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task HandleHintTransferQuery_UserIsSuper_ReturnEmptyAllowTransfer()
        {
            //// Arrange
            user.LevelId.Returns(UserLevel.Super.Value);
            var message = new HintTransferQuery(user);

            //// Act
            var act = await handler.Handle(message);

            //// Assert
            Assert.Equal(string.Empty, act.AllowTransfer);
        }

        [Fact]
        public async Task HandleHintTransferQuery_UserIsNotSuper_CallService()
        {
            //// Arrange
            user.LevelId.Returns(UserLevel.Agent.Value);
            var message = new HintTransferQuery(user);
            cashBalanceTransferService.GetAllowTransfer(Arg.Any<int>()).Returns(string.Empty);

            //// Act
            await handler.Handle(message);

            //// Assert
            await cashBalanceTransferService.Received().GetAllowTransfer(Arg.Any<int>());
        }

        [Fact]
        public async Task HandleHintTransferQuery_UserIsNotSuper_ReturnCorrectAllowTransfer()
        {
            //// Arrange
            var expectedAllowTransfer = "testing...";
            user.LevelId.Returns(UserLevel.Agent.Value);
            var message = new HintTransferQuery(user);
            cashBalanceTransferService.GetAllowTransfer(Arg.Any<int>()).Returns(expectedAllowTransfer);

            //// Act
            var act = await handler.Handle(message);

            //// Assert
            Assert.Equal(expectedAllowTransfer, act.AllowTransfer);
        }

        [Fact]
        public async Task HandleHintTransferQuery_ShowWarningTransfer_TextFromResource()
        {
            //// Arrange
            var textFromResource = Resources.WarningMultipleTransfer;
            user.LevelId.Returns(UserLevel.Agent.Value);
            var message = new HintTransferQuery(user)
            {
                ShowWarningTransfer = true
            };

            //// Act
            var act = await handler.Handle(message);

            //// Assert
            Assert.Equal(textFromResource, act.WarningTransfer);
        }
    }
}
