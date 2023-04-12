namespace Alpha.Transfer.Tests.Models.AutoTransfer
{
    using Alpha.Transfer.Models.AutoTransfer;
    using System;
    using System.Globalization;
    using System.Threading;
    using Xunit;

    [Trait("Category", "AutoTransfer")]
    public class AutoTransferMonitoringModelTest
    {
        [Theory]
        [InlineData("Lu")]
        [InlineData("")]
        public void FirstLastName_HasFirstName_ReturnsCorrectValue(string lastName)
        {
            // Arrange
            var model = new AutoTransferMonitoringModel()
            {
                FirstName = "master01",
                LastName = lastName
            };


            // Act
            var actualValue = string.IsNullOrWhiteSpace(lastName) ? model.FirstName : $"{model.FirstName}\\{lastName}";

            // Assert
            Assert.Equal(model.FirstLastName, actualValue);
        }

        [Theory]
        [InlineData("Lu")]
        [InlineData("")]
        public void FirstLastName_NotHasFirstName_ReturnsCorrectValue(string lastName)
        {
            // Arrange
            var model = new AutoTransferMonitoringModel()
            {
                FirstName = "",
                LastName = lastName

            };


            // Act
            var actualValue = lastName;

            // Assert
            Assert.Equal(model.FirstLastName, actualValue);
        }



    }
}
