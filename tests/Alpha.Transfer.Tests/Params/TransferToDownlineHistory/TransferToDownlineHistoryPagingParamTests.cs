namespace Alpha.Transfer.Tests.Params.TransferToDownlineHistory
{
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Constants;
    using System;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class TransferToDownlineHistoryPagingParamTests
    {
        [Theory]
        [InlineData(-1, DefaultVariablesTest.PageSize)]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        public void PageSize_Always_ReturnPageSizeSetByCtor(int pageSize, int expectedValue)
        {
            // Arrange
            var param = new TransferToDownlineHistoryPagingParam(0, string.Empty, new DateTime(), new DateTime(), 0, pageSize, 0);

            // Act
            var actualValue = param.PageSize;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(-1, DefaultVariablesTest.PageIndex)]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        public void PageIndex_Always_ReturnPageIndexSetByCtor(int pageIndex, int expectedValue)
        {
            // Arrange
            var param = new TransferToDownlineHistoryPagingParam(0, string.Empty, new DateTime(), new DateTime(), 0, 0, pageIndex);

            // Act
            var actualValue = param.PageIndex;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}