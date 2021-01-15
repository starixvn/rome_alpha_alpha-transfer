namespace Alpha.Transfer.Tests.Models.TransferToDownlineHistory
{
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using System;
    using System.Globalization;
    using System.Threading;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class TransferToDownlineHistoryModelTests
    {
        public TransferToDownlineHistoryModelTests()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Theory]
        [InlineData(0, "Deposit")]
        [InlineData(1, "Withdrawal")]
        public void TransferTypeName_Always_ReturnsCorrectValue(byte transferType, string expectedValue)
        {
            // Arrange
            var model = new TransferToDownlineHistoryModel(new DateTime(), string.Empty, transferType, 0, 0, 0, string.Empty);

            // Act
            var actualValue = model.TransferTypeName;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void TransferTypeName_InitWithDefaultCtor_ReturnsDeposit()
        {
            // Arrange
            var model = new TransferToDownlineHistoryModel();

            // Act
            var actualValue = model.TransferTypeName;

            // Assert
            Assert.Equal("Deposit", actualValue);
        }
    }
}