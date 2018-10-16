namespace Alpha.Transfer.Site.Tests.ViewModels.TransferAllAndClearCredit
{
    using Site.ViewModels.TransferAllAndClearCredit;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    public class TransferAllAndClearCreditViewModelTests
    {
        [Fact]
        public void GetOutstandingText_NullOutstanding_ReturnNA()
        {
            // Arrange
            var expectedValue = "N/A";

            // Act
            var actualValue = TransferAllAndClearCreditViewModel.GetOutstandingText(null);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetOutstandingText_HasOutstanding_ReturnCorrectValue()
        {
            // Arrange
            var outstanding = 1000;
            var expectedValue = "1,000.00";

            // Act
            var actualValue = TransferAllAndClearCreditViewModel.GetOutstandingText(outstanding);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
