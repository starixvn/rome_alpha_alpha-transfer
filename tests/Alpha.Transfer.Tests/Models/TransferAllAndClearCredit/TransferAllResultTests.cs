namespace Alpha.Transfer.Tests.Models.TransferAllAndClearCredit
{
    using System.Linq;
    using Transfer.Models.TransferAllAndClearCredit;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    public class TransferAllResultTests
    {
        [Theory]
        [InlineData(0, true)]
        [InlineData(1, false)]
        public void IsSuccess_Always_ReturnsCorrectValue(int errorCode, bool expectedValue)
        {
            // Arrange
            var model = new TransferAllResult(Enumerable.Empty<TransferStep>(), errorCode, string.Empty);

            // Act
            var actualValue = model.IsSuccess;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
