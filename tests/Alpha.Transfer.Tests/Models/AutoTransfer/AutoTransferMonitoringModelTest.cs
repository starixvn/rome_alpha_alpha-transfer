namespace Alpha.Transfer.Tests.Models.AutoTransfer
{
    using Alpha.Transfer.Models.AutoTransfer;
    using Xunit;

    [Trait("Category", "AutoTransfer")]
    public class AutoTransferMonitoringModelTest
    {
        [Theory]
        [InlineData("First", "", "First")]
        [InlineData("First", "Last", "First/ Last")]
        [InlineData("", "Last", "Last")]
        [InlineData("", "", "")]
        public void FirstLastName_HasFirstName_ReturnsCorrectValue(string firstName, string lastName, string expectedValue)
        {
            // Arrange
            var model = new AutoTransferMonitoringModel()
            {
                FirstName = firstName,
                LastName = lastName
            };

            // Act
            var actualValue = model.FirstLastName;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
