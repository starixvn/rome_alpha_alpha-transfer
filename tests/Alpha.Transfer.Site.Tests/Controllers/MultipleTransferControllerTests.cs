namespace Alpha.Transfer.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using Requests.MultipleTransfer;
    using Site.Controllers;
    using Sunplus.Agent.Model.Entities;
    using Transfer.Models;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    public class MultipleTransferControllerTests
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;
        private readonly IUser user;
        private readonly MultipleTransferController controller;

        public MultipleTransferControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            user = Substitute.For<IUser>();
            controller = new MultipleTransferController(mediator, userSession);
        }

        [Fact]
        public void CustomerList_Always_CallsMediatorSendQuery()
        {
            // Arrange
            user.LevelId.Returns((byte)3);
            var message = new CustomerListQuery(user);

            // Act
            controller.CustomerList(message);

            // Assert
            mediator.Received().Send(message);
        }

    }
}
