namespace Alpha.Transfer.Tests.Extensions
{
    using Alpha.Transfer.Extensions;
    using System;
    using Xunit;

    [Trait("Category", "TranferToDownlineHistory")]
    public class DateTimeExtensionsTests
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(12, 0, 0)]
        [InlineData(13, 0, 0)]
        [InlineData(23, 59, 59)]
        public void ToShortDayWithSlash_Always_ReturnsShortDayWithSlash(int hour, int minute, int second)
        {
            // Arrange
            var dateTime = new DateTime(2016, 11, 24, hour, minute, second);
            var expectedDay = "11/24/2016";

            // Act
            var actualDay = dateTime.ToShortDateWithSlash();

            // Assert
            Assert.Equal(expectedDay, actualDay);
        }

        [Theory]
        [InlineData(0, 0, 0, "11/24/2016 12:00:00 AM")]
        [InlineData(1, 0, 0, "11/24/2016 01:00:00 AM")]
        [InlineData(11, 59, 59, "11/24/2016 11:59:59 AM")]
        [InlineData(12, 0, 0, "11/24/2016 12:00:00 PM")]
        [InlineData(13, 0, 0, "11/24/2016 01:00:00 PM")]
        [InlineData(23, 59, 59, "11/24/2016 11:59:59 PM")]
        public void ToLongDateWithSlash_Always_ReturnsLongDayWithSlash(int hour, int minute, int second, string expectedDay)
        {
            // Arrange
            var dateTime = new DateTime(2016, 11, 24, hour, minute, second);

            // Act
            var actualDay = dateTime.ToLongDateWithSlash();

            // Assert
            Assert.Equal(expectedDay, actualDay);
        }

        [Theory]
        [InlineData(0, 0, 0, "11/24/2016 00:00:00")]
        [InlineData(1, 0, 0, "11/24/2016 01:00:00")]
        [InlineData(11, 59, 59, "11/24/2016 11:59:59")]
        [InlineData(12, 0, 0, "11/24/2016 12:00:00")]
        [InlineData(13, 0, 0, "11/24/2016 13:00:00")]
        [InlineData(23, 59, 59, "11/24/2016 23:59:59")]
        public void ToLongDateWithSlash24HFormat_Always_ReturnsLongDateWithSlash24HFormat(int hour, int minute, int second, string expectedDay)
        {
            // Arrange
            var dateTime = new DateTime(2016, 11, 24, hour, minute, second);

            // Act
            var actualDay = dateTime.ToLongDateWithSlash24HFormat();

            // Assert
            Assert.Equal(expectedDay, actualDay);
        }
    }
}