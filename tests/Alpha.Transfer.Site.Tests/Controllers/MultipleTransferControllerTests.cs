namespace Alpha.Transfer.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using Requests.MultipleTransfer;
    using Site.Controllers;
    using Transfer.Models;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    public class MultipleTransferControllerTests
    {
        private readonly IMediator mediator;
        private readonly IUser user;
        private readonly MultipleTransferController controller;

        public MultipleTransferControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            controller = new MultipleTransferController(mediator);
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
