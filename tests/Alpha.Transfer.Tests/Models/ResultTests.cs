namespace Alpha.Transfer.Tests.Models
{
    using Extensions;
    using Transfer.Models;
    using Xunit;

    public class ResultTests
    {
        [Theory]
        [InlineData(0, true)]
        [InlineData(1, false)]
        [InlineData(-1, false)]
        [InlineData(int.MinValue, false)]
        [InlineData(int.MaxValue, false)]
        public void IsSuccessful_Always_ReturnsCorrectValue(int code, bool expectedIsSuccessful)
        {
            // Arrange
            var result = new Result
            {
                Code = code
            };

            // Act
            var actualIsSuccessful = result.IsSuccessful;

            // Assert
            Assert.Equal(expectedIsSuccessful, actualIsSuccessful);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(-1, true)]
        [InlineData(int.MinValue, true)]
        [InlineData(int.MaxValue, true)]
        public void IsFailed_Always_ReturnsCorrectValue(int code, bool expectedIsFailed)
        {
            // Arrange
            var result = new Result
            {
                Code = code
            };

            // Act
            var actualIsFailed = result.IsFailed;

            // Assert
            Assert.Equal(expectedIsFailed, actualIsFailed);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, false)]
        [InlineData(-1, false)]
        [InlineData(int.MinValue, false)]
        [InlineData(int.MaxValue, false)]
        public void IsOk_Always_ReturnsCorrectValue(int code, bool expectedIsOk)
        {
            // Arrange
            var result = new Result
            {
                Code = code
            };

            // Act
            var actualIsOk = result.IsOk;

            // Assert
            Assert.Equal(expectedIsOk, actualIsOk);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(-1, true)]
        [InlineData(int.MinValue, true)]
        [InlineData(int.MaxValue, true)]
        public void IsNotOk_Always_ReturnsCorrectValue(int code, bool expectedIsNotOk)
        {
            // Arrange
            var result = new Result
            {
                Code = code
            };

            // Act
            var actualIsNotOk = result.IsNotOk;

            // Assert
            Assert.Equal(expectedIsNotOk, actualIsNotOk);
        }

        [Fact]
        public void CreateErrorResult_WithCode_ReturnsCorrectModel()
        {
            // Arrange
            var expectModel = new Result
            {
                Code = 2,
                Message = string.Empty
            };

            // Act
            var actualModel = Result.CreateErrorResult(string.Empty, 2);

            // Assert
            AssertExtension.Equal(expectModel, actualModel);
        }

        [Fact]
        public void CreateErrorResult_WithOnlyMessage_ReturnsCorrectModel()
        {
            // Arrange
            var expectModel = new Result
            {
                Code = 1,
                Message = string.Empty
            };

            // Act
            var actualModel = Result.CreateErrorResult(string.Empty);

            // Assert
            AssertExtension.Equal(expectModel, actualModel);
        }
    }
}
