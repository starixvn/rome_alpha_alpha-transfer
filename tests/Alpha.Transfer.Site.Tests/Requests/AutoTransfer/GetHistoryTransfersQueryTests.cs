using Alpha.Transfer.Models;
using Alpha.Transfer.Site.Requests.AutoTransfer;
using Xunit;

namespace Alpha.Transfer.Site.Tests.RequestsTest.AutoTransfer
{
    [Trait("Category", "AutoTransfer")]
    public class GetHistoryTransfersQueryTests
    {
        private readonly IUser user;

        public GetHistoryTransfersQueryTests()
        {
            user = NSubstitute.Substitute.For<IUser>();
        }

        [Fact]
        public void IsValid_InvalidCustId_ReturnsFalse()
        {
            // Arrange
            var model = new GetHistoryTransfersQuery(user);

            // Act
            var actualValue = model.IsValid();

            // Assert
            Assert.False(actualValue);
        }
    }
}