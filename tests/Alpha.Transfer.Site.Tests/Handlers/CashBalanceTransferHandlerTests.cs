namespace Alpha.Transfer.Site.Tests.Handlers
{
    using System.Threading.Tasks;
    using AppResources;
    using Extensions;
    using NSubstitute;
    using Requests.CashBalanceTransfer;
    using Services;
    using Site.Handlers;
    using Transfer.Models;
    using Transfer.Models.CashBalanceTransfer;
    using ViewModels.CashBalanceTransfer;
    using Xunit;

    [Trait("Category", "CashBalanceTransfer")]
    public class CashBalanceTransferHandlerTests
    {
        private readonly IUser user;
        private readonly CashBalanceTransferService service;
        private readonly CashBalanceTransferHandler handler;

        public CashBalanceTransferHandlerTests()
        {
            user = Substitute.For<IUser>();
            service = Substitute.For<CashBalanceTransferService>();
            handler = new CashBalanceTransferHandler(service);
        }

        [Fact]
        public async Task HandlerCashBalanceTransferQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            int custId = 1176083;
            var message = new CashBalanceTransferQuery { CustId = custId };
            var cashBalanceTransfer = new CashBalanceTransfer(
                1176083,
                3,
                "master1",
                67599433.9247M,
                1101240.3283M,
                342552.00M,
                900000002.00M);

            var expectedViewModel = new CashBalanceTransferViewModel(
                cashBalanceTransfer.UplineCashBalance,
                cashBalanceTransfer.CustId,
                cashBalanceTransfer.UserName,
                Enumeration.FromValue<UserLevel>(cashBalanceTransfer.RoleId),
                cashBalanceTransfer.CashBalance,
                cashBalanceTransfer.Outstanding,
                cashBalanceTransfer.GivenCredit);

            service.GetCashBalanceTransferAsync(custId).Returns(cashBalanceTransfer);

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
            var message = new CashBalanceTransferQuery { CustId = custId };
            var cashBalanceTransfer = new CashBalanceTransfer(
                1176083,
                2,
                "agent1",
                0,
                1101240.3283M,
                1101240.3283M,
                900000002.00M);

            var hintFromResource = Resources.HintNotAvailableToTransfer;

            service.GetCashBalanceTransferAsync(custId).Returns(cashBalanceTransfer);

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
            var message = new CashBalanceTransferQuery { CustId = custId };
            var cashBalanceTransfer = new CashBalanceTransfer(
                1176083,
                1,
                "member1",
                0,
                1101240.3283M,
                1101240.3283M,
                900000002.00M);

            var levelName = Resources.Member;

            service.GetCashBalanceTransferAsync(custId).Returns(cashBalanceTransfer);

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
            var message = new CashBalanceTransferQuery { CustId = custId };
            var cashBalanceTransfer = new CashBalanceTransfer(
                1176083,
                2,
                "agent1",
                0,
                1101240.3283M,
                1101240.3283M,
                900000002.00M);

            var levelName = Resources.Agent;

            service.GetCashBalanceTransferAsync(custId).Returns(cashBalanceTransfer);

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

            service.AddCashBalanceTransferAsync(
                command.UplineId,
                command.CustId,
                command.Amount,
                command.UplineId,
                command.UplineName,
                command.SubAccId,
                command.IP,
                command.LanguageKey).Returns(expectedResult);

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
            service.GetAllowTransfer(Arg.Any<int>()).Returns(string.Empty);

            //// Act
            var act = await handler.Handle(message);

            //// Assert
            await service.Received().GetAllowTransfer(Arg.Any<int>());
        }

        [Fact]
        public async Task HandleHintTransferQuery_UserIsNotSuper_ReturnCorrectAllowTransfer()
        {
            //// Arrange
            var expectedAllowTransfer = "testing...";
            user.LevelId.Returns(UserLevel.Agent.Value);
            var message = new HintTransferQuery(user);
            service.GetAllowTransfer(Arg.Any<int>()).Returns(expectedAllowTransfer);

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
