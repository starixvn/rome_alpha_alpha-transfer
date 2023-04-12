using Alpha.Transfer.Site.Requests.AutoTransfer;
using Alpha.Transfer.Site.Tests.Fixtures;
using Xunit;

namespace Alpha.Transfer.Site.Tests.RequestsTest.AutoTransfer
{
    [Trait("Category", "AutoTransfer")]
    public class GetHistoryTransfersQueryTest
    {
        private readonly UserFixture userFixture;

        public GetHistoryTransfersQueryTest()
        {
            userFixture = new UserFixture();
        }


        [Fact]
        public void ValidCustId_Always_ReturnsIsValid()
        {
            // Arrange
            var model = new GetHistoryTransfersQuery(userFixture.User);

            // Act
            var actualValue = model.IsValid();

            // Assert
            Assert.True(actualValue);
        }
    }
}