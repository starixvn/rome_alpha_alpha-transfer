namespace Alpha.Transfer.Tests.Services
{
    using System.Linq;
    using Alpha.Transfer.Params.MultipleTransfer;
    using Fanex.Data.Repository;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Transfer.Criteria.MultipleTransfer;
    using Transfer.Models;
    using Transfer.Models.MultipleTransfer;
    using Transfer.Services;
    using Xunit;

    [Trait("Category", "MultipleTransfer")]
    [Collection("AnyValueCreatorCollection")]
    public class MultipleTransferServiceImplTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly IDynamicRepository repository;
        private readonly MultipleTransferServiceImpl serviceImpl;
        private readonly CompareLogic compareLogic;

        public MultipleTransferServiceImplTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;

            repository = Substitute.For<IDynamicRepository>();
            serviceImpl = new MultipleTransferServiceImpl(repository);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetCustomerList_Always_ReturnsDataFromRepository()
        {
            // Arrange
            var transferResult = new CustomerListModel(Enumerable.Empty<CustomerInformation>(), 1);
            //repository.GetCustomerList(Arg.Any<GetCustomerListCriteria>()).Returns(transferResult);

            // Act
            var actualData = serviceImpl.GetCustomerList(new GetCustomerListParam(UserLevel.Agent, 1, string.Empty, 1, 1, false, false, 1));

            // Assert
            Assert.True(compareLogic.Compare(transferResult, actualData).AreEqual);
        }
    }
}