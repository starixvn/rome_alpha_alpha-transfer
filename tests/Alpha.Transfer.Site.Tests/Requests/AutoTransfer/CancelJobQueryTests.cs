using Alpha.Transfer.Site.Requests.AutoTransfer;
using Alpha.Transfer.Site.Tests.Fixtures;
using Xunit;

namespace Alpha.Transfer.Site.Tests.RequestsTest.AutoTransfer
{
    [Trait("Category", "AutoTransfer")]
    public class CancelJobQueryTests
    {
        private readonly UserFixture userFixture;

        public CancelJobQueryTests()
        {
            userFixture = new UserFixture();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void IsValid_ValidQuery_ReturnsTrue(string searchAccountIds)
        {
            // Arrange
            var message = new CancelJobQuery(userFixture.User);

            // Act
            var actualValue = message.IsValid();

            // Assert
            Assert.False(actualValue);
        }
    }
}