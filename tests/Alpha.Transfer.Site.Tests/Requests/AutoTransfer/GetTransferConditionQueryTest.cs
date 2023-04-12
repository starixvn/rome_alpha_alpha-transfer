using Alpha.Transfer.Site.Requests.AutoTransfer;
using Alpha.Transfer.Site.Tests.Fixtures;
using Xunit;

namespace Alpha.Transfer.Site.Tests.RequestsTest.AutoTransfer
{
    [Trait("Category", "AutoTransfer")]
    public class GetTransferConditionQueryTest
    {
        private readonly UserFixture userFixture;

        public GetTransferConditionQueryTest()
        {
            userFixture = new UserFixture();
        }


        [Fact]
        public void ValidCustIds_Always_ReturnsIsValid()
        {
            // Arrange

            var message = new UpdateMultipleQuery(userFixture.User);

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