namespace Alpha.Transfer.Tests.Params.TransferToDownlineHistory
{
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Alpha.Transfer.Utils;
    using Constants;
    using System;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class TransferToDownlineHistoryParamTests
    {
        public TransferToDownlineHistoryParamTests()
        {
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
        }

        [Fact]
        public void CustId_Always_ReturnsCustIdSetByCtor()
        {
            // Arrange
            var custId = 1;
            var param = new TransferToDownlineHistoryParam(custId, string.Empty, new DateTime(), new DateTime(), 0, 0);

            // Act
            var actualCustId = param.CustId;

            // Assert
            Assert.Equal(custId, actualCustId);
        }

        [Fact]
        public void UserName_Always_ReturnUserNameSetByCtor()
        {
            // Arrange
            var userName = "username";
            var param = new TransferToDownlineHistoryParam(0, userName, new DateTime(), new DateTime(), 0, 0);

            // Act
            var actualUserName = param.UserName;

            // Assert
            Assert.Equal(userName, actualUserName);
        }

        [Fact]
        public void FromDate_MinValue_ReturnsSystemTimeTody()
        {
            // Arrange
            var param = new TransferToDownlineHistoryParam(0, string.Empty, DateTime.MinValue, new DateTime(), 0, 0);

            // Act
            var actualFromDate = param.FromDate;

            // Assert
            Assert.Equal(SystemTime.Today, actualFromDate);
        }

        [Fact]
        public void FromDate_ValidDateTime_ReturnsFromDateSetByCtor()
        {
            // Arrange
            var fromDate = DefaultVariablesTest.CurrentDate;
            var param = new TransferToDownlineHistoryParam(0, string.Empty, fromDate, new DateTime(), 0, 0);

            // Act
            var actualFromDate = param.FromDate;

            // Assert
            Assert.Equal(fromDate, actualFromDate);
        }

        [Fact]
        public void ToDate_MinValue_ReturnsSystemTimeTody()
        {
            // Arrange
            var param = new TransferToDownlineHistoryParam(0, string.Empty, new DateTime(), DateTime.MinValue, 0, 0);

            // Act
            var actualToDate = param.ToDate;

            // Assert
            Assert.Equal(SystemTime.Today, actualToDate);
        }

        [Fact]
        public void ToDate_ValidDateTime_ReturnsToDateSetByCtor()
        {
            // Arrange
            var toDate = DefaultVariablesTest.CurrentDate;
            var param = new TransferToDownlineHistoryParam(0, string.Empty, new DateTime(), toDate, 0, 0);

            // Act
            var actualToDate = param.ToDate;

            // Assert
            Assert.Equal(toDate, actualToDate);
        }

        [Theory]
        [InlineData(-10, DefaultVariablesTest.PageSize)]
        [InlineData(0, DefaultVariablesTest.PageSize)]
        [InlineData(10, 10)]
        public void PageSize_Always_ReturnsPageSizeSetByCtor(int pageSize, int expectedPageSize)
        {
            // Arrange
            var param = new TransferToDownlineHistoryParam(0, string.Empty, new DateTime(), new DateTime(), pageSize, 0);

            // Act
            var actualPageSize = param.PageSize;

            // Assert
            Assert.Equal(expectedPageSize, actualPageSize);
        }

        [Theory]
        [InlineData(-1, DefaultVariablesTest.PageIndex)]
        [InlineData(0, DefaultVariablesTest.PageIndex)]
        [InlineData(1, 1)]
        public void Page_Always_ReturnsPageSetByCtor(int page, int expectedPage)
        {
            // Arrange
            var param = new TransferToDownlineHistoryParam(0, string.Empty, new DateTime(), new DateTime(), 0, page);

            // Act
            var actualPage = param.Page;

            // Assert
            Assert.Equal(expectedPage, actualPage);
        }
    }
}