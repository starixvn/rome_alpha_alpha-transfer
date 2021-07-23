namespace Alpha.Transfer.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using NSubstitute;
    using Requests.TransferAllAndClearCredit;
    using Site.Controllers;
    using Sunplus.Agent.Model.Entities;
    using Transfer.Models;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    public class TransferAllAndClearCreditControllerTests
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;
        private readonly IUser user;
        private readonly TransferAllAndClearCreditController controller;

        public TransferAllAndClearCreditControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            user = Substitute.For<IUser>();
            controller = new TransferAllAndClearCreditController(mediator, userSession);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendQuery()
        {
            // Arrange
            user.Language.Returns("en-US");
            user.LevelId.Returns((byte)3);
            var message = new CalculationQuery(user)
            {
                CustId = 1,
                CustName = "CustName",
                CustLevelId = 2
            };

            // Act
            controller.Index(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void GetTransferResult_Always_CallsMediatorSendQuery()
        {
            // Arrange
            user.Language.Returns("en-US");
            user.LevelId.Returns((byte)3);
            var message = new CalculationQuery(user)
            {
                CustId = 1,
                CustName = "CustName",
                CustLevelId = 2
            };

            // Act
            controller.TransferResult(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Index_NullQuery_ReturnsInvalidPage()
        {
            // Act
            var actualValue = controller.Index(null);

            // Assert
            Assert.Equal("~/Views/Shared/InvalidPage.cshtml", ((ViewResult)actualValue).ViewName);
        }

        [Fact]
        public void Index_InvalidQuery_ReturnsInvalidPage()
        {
            // Arrange
            user.Language.Returns("en-US");
            user.LevelId.Returns((byte)3);
            var message = new CalculationQuery(user)
            {
                CustId = 1,
                CustName = "CustName",
                CustLevelId = 4
            };

            // Act
            var actualValue = controller.Index(message);

            // Assert
            Assert.Equal("~/Views/Shared/InvalidPage.cshtml", ((ViewResult)actualValue).ViewName);
        }

        [Fact]
        public void TransferResult_Always_CallsMediatorSendQuery()
        {
            // Arrange
            user.Language.Returns("en-US");
            user.LevelId.Returns((byte)3);
            var message = new TransferResultQuery(user)
            {
                CustId = 1,
                CustName = "CustName",
                CustLevelId = 2
            };

            // Act
            controller.TransferResult(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void TransferResult_NullQuery_ReturnsInvalidPage()
        {
            // Act
            var actualValue = controller.TransferResult((TransferResultQuery)null);

            // Assert
            Assert.Equal("~/Views/Shared/InvalidPage.cshtml", ((ViewResult)actualValue).ViewName);
        }

        [Fact]
        public void TransferResult_InvalidQuery_ReturnsInvalidPage()
        {
            // Arrange
            user.Language.Returns("en-US");
            user.LevelId.Returns((byte)3);
            var message = new TransferResultQuery(user)
            {
                CustId = 1,
                CustName = "CustName",
                CustLevelId = 4
            };

            // Act
            var actualValue = controller.TransferResult(message);

            // Assert
            Assert.Equal("~/Views/Shared/InvalidPage.cshtml", ((ViewResult)actualValue).ViewName);
        }
    }
}
