namespace Alpha.Transfer.Site.Tests.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AppResources;
    using MediatR;
    using NSubstitute;
    using Requests.CashBalanceTransfer;
    using Site.Controllers;
    using Sunplus.Agent.Model.Entities;
    using Transfer.Models;
    using Xunit;

    [Trait("Category", "CashBalanceTransfer")]
    public class CashBalanceTransferControllerTests
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;
        private readonly IUser user;
        private readonly CashBalanceTransferController controller;

        public CashBalanceTransferControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            user = Substitute.For<IUser>();
            controller = new CashBalanceTransferController(mediator, userSession);
        }

        [Fact]
        public async Task CashBalance_Always_CallsMediatorCashBalanceTransferQuery()
        {
            // Arrange
            var message = new CashBalanceTransferQuery { CustId = 1176083 };
            userSession.IsAllDayBalTransfer.Returns(true);

            // Act
            await controller.CashBalance(message);

            // Assert
            await mediator.Received().SendAsync(message);
        }

        [Theory]
        [InlineData(-1176083)]
        [InlineData(0)]
        public async Task CashBalance_QueryIsNotValid_ReturnInvalidPageView(int custId)
        {
            // Arrange
            var message = new CashBalanceTransferQuery { CustId = custId };

            // Act
            var actualView = await controller.CashBalance(message) as ViewResult;

            // Assert
            Assert.Equal("~/Views/Shared/InvalidPage.cshtml", actualView.ViewName);
        }

        [Fact]
        public async Task CashBalance_QueryInvalidIsAllDayBalTransfer_ReturnInvalidPageView()
        {
            // Arrange
            var message = new CashBalanceTransferQuery { CustId = 1176083 };
            userSession.IsAllDayBalTransfer.Returns(false);

            // Act
            var actualView = await controller.CashBalance(message) as ViewResult;

            // Assert
            Assert.Equal("~/Views/Shared/InvalidPage.cshtml", actualView.ViewName);
        }

        [Fact]
        public async Task CashBalance_Always_CallsMediatorCashBalanceTransferCommand()
        {
            // Arrange
            user.Id.Returns(1176070);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");

            var command = new CashBalanceTransferCommand(user)
            {
                CustId = 1176083,
                Amount = 1101240.32M
            };

            userSession.IsAllDayBalTransfer.Returns(true);
            userSession.IsInternal.Returns(false);

            // Act
            await controller.CashBalance(command);

            // Assert
            await mediator.Received().SendAsync(command);
        }

        [Theory]
        [InlineData(-1176083, 1101240.32, 1176070)]
        [InlineData(0, 1101240.32, 1176070)]
        [InlineData(1176083, 0, 1176070)]
        [InlineData(1176083, 101240.32, -1176070)]
        [InlineData(1176083, 101240.32, 0)]
        public async Task CashBalance_CommandIsNotValid_ReturnBadRequestResult(int custId, decimal amount, int uplineId)
        {
            // Arrange
            user.Id.Returns(uplineId);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");

            var command = new CashBalanceTransferCommand(user)
            {
                CustId = custId,
                Amount = amount
            };

            var expectedResult = Result.CreateErrorResult(Resources.BadRequest, 400);

            // Act
            var actualResult = await controller.CashBalance(command) as JsonResult;

            // Assert
            var actualData = (Result)actualResult.Data;

            Assert.True(actualData.Code == expectedResult.Code &&
                actualData.Message == expectedResult.Message);
        }

        [Fact]
        public async Task CashBalance_CommandInvalidIsAllDayBalTransfer_ReturnBadRequestResult()
        {
            // Arrange
            user.Id.Returns(1176070);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");

            var command = new CashBalanceTransferCommand(user)
            {
                CustId = 1176083,
                Amount = 1101240.32M
            };

            var expectedResult = Result.CreateErrorResult(Resources.BadRequest, 400);

            userSession.IsAllDayBalTransfer.Returns(false);

            // Act
            var actualResult = await controller.CashBalance(command) as JsonResult;

            // Assert
            var actualData = (Result)actualResult.Data;

            Assert.True(actualData.Code == expectedResult.Code &&
                actualData.Message == expectedResult.Message);
        }

        [Fact]
        public async Task CashBalance_CommandIsInternal_ReturnBadRequestResult()
        {
            // Arrange
            user.Id.Returns(1176070);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");

            var command = new CashBalanceTransferCommand(user)
            {
                CustId = 1176083,
                Amount = 1101240.32M
            };

            var expectedResult = Result.CreateErrorResult(Resources.NotAuthorized, 403);

            userSession.IsAllDayBalTransfer.Returns(true);
            userSession.IsInternal.Returns(true);

            // Act
            var actualResult = await controller.CashBalance(command) as JsonResult;

            // Assert
            var actualData = (Result)actualResult.Data;

            Assert.True(actualData.Code == expectedResult.Code &&
                actualData.Message == expectedResult.Message);
        }

        [Theory]
        [InlineData("zh-TW")]
        [InlineData("zh-CN")]
        [InlineData("th-TH")]
        public async Task Hint_ValidLanguages_ReturnPageView(string language)
        {
            // Arrange
            userSession.IsAllDayBalTransfer.Returns(true);
            user.Language.Returns(language);
            user.Id.Returns(1176070);

            var message = new HintTransferQuery(user);

            // Act
            var actualView = await controller.Hint(message) as ViewResult;

            // Assert
            Assert.Equal("Hint." + language, actualView.ViewName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("abcdef")]
        public async Task Hint_InvalidLanguages_ReturnPageView(string language)
        {
            // Arrange
            userSession.IsAllDayBalTransfer.Returns(true);
            user.Language.Returns(language);
            user.Id.Returns(1176070);

            var message = new HintTransferQuery(user);

            // Act
            var actualView = await controller.Hint(message) as ViewResult;

            // Assert
            Assert.Equal("Hint", actualView.ViewName);
        }

        [Fact]
        public async Task Hint_InvalidIsAllDayBalTransfer_ReturnInvalidPageView()
        {
            // Arrange
            userSession.IsAllDayBalTransfer.Returns(false);
            user.Language.Returns("en-US");
            user.Id.Returns(1176070);

            var message = new HintTransferQuery(user);

            // Act
            var actualView = await controller.Hint(message) as ViewResult;

            // Assert
            Assert.Equal("~/Views/Shared/InvalidPage.cshtml", actualView.ViewName);
        }
    }
}
