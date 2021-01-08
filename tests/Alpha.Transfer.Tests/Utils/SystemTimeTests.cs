namespace Alpha.Transfer.Tests.Utils
{
    using Alpha.Transfer.Utils;
    using Constants;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class SystemTimeTests
    {
        [Fact]
        public void Now_Always_ReturnsValueSetToNowProperty()
        {
            // Arrange
            var expectedDate = DefaultVariablesTest.CurrentDate;
            SystemTime.Today = expectedDate;

            // Act
            var actualDate = SystemTime.Today;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }
    }
}