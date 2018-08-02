namespace Alpha.Transfer.Tests.Helpers
{
    using Transfer.Helpers;
    using Xunit;

    [Trait("Category", "Transfer")]
    public class JsonHelperTests
    {
        [Fact]
        public void ToJson_Always_ReturnsCorrectValue()
        {
            // Arrange
            var expectedValue = "{\"value\":1}";

            // Act
            var actualValue = JsonHelper.ToJson(new { value = 1 });

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
