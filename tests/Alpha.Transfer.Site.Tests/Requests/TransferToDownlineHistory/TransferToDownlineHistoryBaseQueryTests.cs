using Alpha.Transfer.Site.Requests.TransferToDownlineHistory;
using Alpha.Transfer.Site.Tests.Fixtures;
using Xunit;

namespace Alpha.Transfer.Site.Tests.RequestsTest.TransferToDownlineHistory
{
    [Trait("Category", "TransferToDownlineHistory")]
    public class TransferToDownlineHistoryBaseQueryTests
    {
        private readonly UserFixture userFixture;

        public TransferToDownlineHistoryBaseQueryTests()
        {
            userFixture = new UserFixture();
        }

        [Fact]
        public void LimitedDateToShowInfo_Always_ReturnCorrectValue()
        {
            // Arrange
            var expectedLimitedDateToShowInfo = userFixture.User.LimitedDateToShowInfo;
            var query = new TransferToDownlineHistoryBaseQuery(userFixture.User);

            // Act
            var actualLimitedDateToShowInfo = query.LimitedDateToShowInfo;

            // Assert
            Assert.Equal(expectedLimitedDateToShowInfo, actualLimitedDateToShowInfo);
        }
    }
}