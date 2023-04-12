using Alpha.Transfer.Site.Requests.AutoTransfer;
using Alpha.Transfer.Site.Tests.Fixtures;
using Xunit;

namespace Alpha.Transfer.Site.Tests.RequestsTest.AutoTransfer
{
    [Trait("Category", "AutoTransfer")]
    public class UpdateMultipleQueryTests
    {
        private readonly UserFixture userFixture;

        public UpdateMultipleQueryTests()
        {
            userFixture = new UserFixture();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void IsValid_NotHaveSearchAccountIds_ReturnsFalse(string searchAccountIds)
        {
            // Arrange
            var message = new UpdateMultipleQuery(userFixture.User)
            {
                SearchAccountIds = searchAccountIds,
            };

            // Act
            var actualValue = message.IsValid();

            // Assert
            Assert.False(actualValue);
        }

        [Fact]
        public void IsValid_HasSearchAccountIdsAndUplineId_ReturnsIsValid()
        {
            // Arrange
            var message = new UpdateMultipleQuery(userFixture.User)
            {
                SearchAccountIds = "1176083,33278659,14373487",
            };

            // Act
            var actualValue = message.IsValid();

            // Assert
            Assert.True(actualValue);
        }
    }
}