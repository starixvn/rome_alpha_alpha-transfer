namespace Alpha.Transfer.Site.Tests.Handlers
{
    using System.Linq;
    using Extensions;
    using NSubstitute;
    using Params.MultipleTransfer;
    using Requests.MultipleTransfer;
    using Services;
    using Site.Handlers;
    using Site.ViewModels.MultipleTransfer;
    using Transfer.Models;
    using Transfer.Models.MultipleTransfer;
    using Xunit;

    [Trait("Category", "MultipleTransfer")]
    public class MultipleTransferHandlerTests
    {
        private readonly IUser user;
        private readonly MultipleTransferService service;
        private readonly MultipleTransferHandler handler;

        public MultipleTransferHandlerTests()
        {
            user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);

            service = Substitute.For<MultipleTransferService>();
            handler = new MultipleTransferHandler(service);
        }

        [Fact]
        public void HandlerCustomerListQuery_Always_ReturnCorrectValue()
        {
            // Arrange
            var query = new CustomerListQuery(user);
            var data = new CustomerListModel(Enumerable.Empty<CustomerInformation>(), 2);
            service.GetCustomerList(Arg.Any<GetCustomerListParam>()).Returns(data);
            var expectValue = new CustomerListViewModel(query, data);

            // Act
            var actualValue = handler.Handle(query);

            // Assert
            AssertExtension.Equal(expectValue, actualValue);
        }
    }
}
