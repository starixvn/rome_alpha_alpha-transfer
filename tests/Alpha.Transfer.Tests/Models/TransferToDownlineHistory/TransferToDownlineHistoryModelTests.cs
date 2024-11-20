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
        [InlineData(0, "Withdrawal")]
        [InlineData(1, "Deposit")]
        public void TransferTypeName_Always_ReturnsCorrectValue(byte transferType, string expectedValue)
        {
            // Arrange
            var model = new TransferToDownlineHistoryModel(new DateTime(), string.Empty, transferType, 0, 0, 0, string.Empty, false);

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
            Assert.Equal("Withdrawal", actualValue);
        }

        [Theory]
        [InlineData(0, 0, 0, "11/24/2016 00:00:00")]
        [InlineData(1, 0, 0, "11/24/2016 01:00:00")]
        [InlineData(11, 59, 59, "11/24/2016 11:59:59")]
        [InlineData(12, 0, 0, "11/24/2016 12:00:00")]
        [InlineData(13, 0, 0, "11/24/2016 13:00:00")]
        [InlineData(23, 59, 59, "11/24/2016 23:59:59")]
        public void FormattedActionDate_Always_ReturnsLongDateWithSlash24HFormat(int hour, int minute, int second, string expectedValue)
        {
            // Arrange
            var dateTime = new DateTime(2016, 11, 24, hour, minute, second);
            var model = new TransferToDownlineHistoryModel(dateTime, string.Empty, 0, 0, 0, 0, string.Empty, false);

            // Act
            var actualValue = model.FormattedActionDate;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0, "0.00")]
        [InlineData(1, "1.00")]
        [InlineData(10, "10.00")]
        [InlineData(100, "100.00")]
        [InlineData(1000, "1,000.00")]
        [InlineData(0.5, "0.50")]
        [InlineData(100.123, "100.12")]
        [InlineData(1000.123, "1,000.12")]
        [InlineData(1.1000, "1.10")]
        [InlineData(1111111.111, "1,111,111.11")]
        public void FormattedTransferAmount_Always_ReturnCorrectValue(decimal transferAmmount, string expectedValue)
        {
            // Arrange
            var model = new TransferToDownlineHistoryModel(new DateTime(), string.Empty, 0, transferAmmount, 0, 0, string.Empty, false);

            // Assert
            var actualValue = model.FormattedTransferAmount;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0, "0.00")]
        [InlineData(1, "1.00")]
        [InlineData(10, "10.00")]
        [InlineData(100, "100.00")]
        [InlineData(1000, "1,000.00")]
        [InlineData(0.5, "0.50")]
        [InlineData(100.123, "100.12")]
        [InlineData(1000.123, "1,000.12")]
        [InlineData(1.1000, "1.10")]
        [InlineData(1111111.111, "1,111,111.11")]
        public void FormattedOldBalance_Always_ReturnCorrectValue(decimal oldBalance, string expectedValue)
        {
            // Arrange
            var model = new TransferToDownlineHistoryModel(new DateTime(), string.Empty, 0, 0, oldBalance, 0, string.Empty, false);

            // Assert
            var actualValue = model.FormattedOldBalance;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0, "0.00")]
        [InlineData(1, "1.00")]
        [InlineData(10, "10.00")]
        [InlineData(100, "100.00")]
        [InlineData(1000, "1,000.00")]
        [InlineData(0.5, "0.50")]
        [InlineData(100.123, "100.12")]
        [InlineData(1000.123, "1,000.12")]
        [InlineData(1.1000, "1.10")]
        [InlineData(1111111.111, "1,111,111.11")]
        public void FormattedNewBalance_Always_ReturnCorrectValue(decimal newBalance, string expectedValue)
        {
            // Arrange
            var model = new TransferToDownlineHistoryModel(new DateTime(), string.Empty, 0, 0, 0, newBalance, string.Empty, false);

            // Assert
            var actualValue = model.FormattedNewBalance;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}