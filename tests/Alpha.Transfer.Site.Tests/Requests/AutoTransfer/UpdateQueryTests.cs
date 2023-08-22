using Alpha.Transfer.Site.Requests.AutoTransfer;
using Alpha.Transfer.Site.Tests.Fixtures;
using Xunit;

namespace Alpha.Transfer.Site.Tests.RequestsTest.AutoTransfer
{
    [Trait("Category", "AutoTransfer")]
    public class UpdateQueryTests
    {
        private readonly UserFixture userFixture;

        public UpdateQueryTests()
        {
            userFixture = new UserFixture();
        }

        [Fact]
        public void IsValid_ValidQuery_ReturnsTrue()
        {
            // Arrange
            var message = new UpdateQuery(userFixture.User) { CustId = 2 };

            // Act
            var actualValue = message.IsValid();

            // Assert
            Assert.True(actualValue);
        }
    }
}