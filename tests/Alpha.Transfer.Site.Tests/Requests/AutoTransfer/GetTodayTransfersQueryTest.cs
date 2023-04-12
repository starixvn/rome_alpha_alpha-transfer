using Alpha.Transfer.Site.Requests.AutoTransfer;
using Alpha.Transfer.Site.Tests.Fixtures;
using Xunit;

namespace Alpha.Transfer.Site.Tests.RequestsTest.AutoTransfer
{
    [Trait("Category", "AutoTransfer")]
    public class GetTodayTransfersQueryTest
    {
        private readonly UserFixture userFixture;

        public GetTodayTransfersQueryTest()
        {
            userFixture = new UserFixture();
        }


        [Fact]
        public void ValidCustId_Always_ReturnsIsValid()
        {
            // Arrange
            var model = new GetTodayTransfersQuery(userFixture.User);

            // Act
            var actualValue = model.IsValid();

            // Assert
            Assert.True(actualValue);
        }
    }
}