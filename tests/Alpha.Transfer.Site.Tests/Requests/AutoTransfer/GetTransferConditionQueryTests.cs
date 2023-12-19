using Alpha.Transfer.Models;
using Alpha.Transfer.Site.Requests.AutoTransfer;
using Alpha.Transfer.Site.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Alpha.Transfer.Site.Tests.RequestsTest.AutoTransfer
{
    [Trait("Category", "AutoTransfer")]
    public class GetTransferConditionQueryTests
    {
        private readonly UserFixture userFixture;

        public GetTransferConditionQueryTests()
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
            var model = new GetTransferConditionQuery
            {
                UplineId = userFixture.User.Id,
                ViewerLevel = userFixture.User.LevelId,
                SearchAccountIds = searchAccountIds
            };

            // Act
            var actualValue = model.IsValid();

            // Assert
            Assert.False(actualValue);
        }

        [Fact]
        public void IsValid_HasSearchAccountIds_ReturnsIsValid()
        {
            // Arrange
            var user = Substitute.For<IUser>();

            var model = new GetTransferConditionQuery
            {
                UplineId = user.Id,
                ViewerLevel = user.LevelId,
                SearchAccountIds = "1176083,33278659,14373487",
            };

            // Act
            var actualValue = model.IsValid();

            // Assert
            Assert.True(actualValue);
        }
    }
}