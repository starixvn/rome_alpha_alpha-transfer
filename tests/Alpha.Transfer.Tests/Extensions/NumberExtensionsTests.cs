namespace Alpha.Transfer.Tests.Extensions
{
    using Alpha.Transfer.Extensions;
    using Xunit;

    [Trait("Category", "TranferToDownlineHistory")]
    public class NumberExtensionsTests
    {
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
        public void FormatWithCommasAlwaysWithDecimalPoint_Always_ReturnCorrectValue(decimal number, string expectedValue)
        {
            // Assert
            var actualValue = number.FormatWithCommasAlwaysWithDecimalPoint();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData(10, "10")]
        [InlineData(100, "100")]
        [InlineData(1000, "1,000")]
        [InlineData(0.5, "0.5")]
        [InlineData(100.123, "100.12")]
        [InlineData(1000.123, "1,000.12")]
        [InlineData(1.1000, "1.1")]
        [InlineData(1111111.111, "1,111,111.11")]
        public void FormatWithCommasOptionalWithDecimalPoint_Always_ReturnCorrectValue(decimal number, string expectedValue)
        {
            // Assert
            var actualValue = number.FormatWithCommasOptionalWithDecimalPoint();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}