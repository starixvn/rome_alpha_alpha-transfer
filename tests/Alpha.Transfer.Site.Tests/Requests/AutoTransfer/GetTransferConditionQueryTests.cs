using Alpha.Transfer.Models;
using Alpha.Transfer.Site.Models;
using Alpha.Transfer.Site.Requests.AutoTransfer;
using Alpha.Transfer.Site.Tests.Fixtures;
using NSubstitute;
using Sunplus.Agent.Model.Entities;
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
            var message = new UpdateMultipleQuery(userFixture.User);

            var model = new GetTransferConditionQuery(message)
            {
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
            var message = new UpdateMultipleQuery(user);

            var model = new GetTransferConditionQuery(message)
            {
                SearchAccountIds = "1176083,33278659,14373487",
            };

            // Act
            var actualValue = model.IsValid();

            // Assert
            Assert.True(actualValue);
        }
    }
}