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
            var param = new TransferToDownlineHistoryParam(custId, string.Empty, new DateTime(), new DateTime());

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
            var param = new TransferToDownlineHistoryParam(0, userName, new DateTime(), new DateTime());

            // Act
            var actualUserName = param.UserName;

            // Assert
            Assert.Equal(userName, actualUserName);
        }

        [Fact]
        public void FromDate_MinValue_ReturnsSystemTimeTody()
        {
            // Arrange
            var param = new TransferToDownlineHistoryParam(0, string.Empty, DateTime.MinValue, new DateTime());

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
            var param = new TransferToDownlineHistoryParam(0, string.Empty, fromDate, new DateTime());

            // Act
            var actualFromDate = param.FromDate;

            // Assert
            Assert.Equal(fromDate, actualFromDate);
        }

        [Fact]
        public void ToDate_MinValue_ReturnsSystemTimeTody()
        {
            // Arrange
            var param = new TransferToDownlineHistoryParam(0, string.Empty, new DateTime(), DateTime.MinValue);

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
            var param = new TransferToDownlineHistoryParam(0, string.Empty, new DateTime(), toDate);

            // Act
            var actualToDate = param.ToDate;

            // Assert
            Assert.Equal(toDate, actualToDate);
        }
    }
}