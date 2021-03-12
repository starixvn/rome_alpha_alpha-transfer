namespace Alpha.Transfer.Tests.Criteria.TransferToDownlineHistory
{
    using Alpha.Transfer.Criteria.TransferToDownlineHistory;
    using Constants;
    using System;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class GetTransferToDownlineHistoryCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var expectedSettingKey = "Age_Rpt_Downline_Transfer_History";
            var criteria = new GetTransferToDownlineHistoryCriteria(
                0,
                string.Empty,
                new DateTime(),
                new DateTime(),
                0,
                0);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        public void IsValid_CustId_ReturnsCorrectValue(int custId, bool expectedValue)
        {
            // Arrange
            var criteria = new GetTransferToDownlineHistoryCriteria(
                custId,
                string.Empty,
                new DateTime(),
                new DateTime(),
                0,
                0);

            // Act
            var actualValue = criteria.IsValid();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void IsValid_FromDateLaterThanToDate_ReturnFalse()
        {
            // Arrange
            var criteria = new GetTransferToDownlineHistoryCriteria(
                0,
                string.Empty,
                new DateTime(2021, 3, 12),
                new DateTime(2021, 3, 11),
                0,
                0);

            // Act
            var actualValue = criteria.IsValid();

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        public void CustId_Always_ReturnsCustIdSetByCtor(int custId, int expectedCustId)
        {
            // Arrange
            var criteria = new GetTransferToDownlineHistoryCriteria(custId, string.Empty, new DateTime(), new DateTime(), 0, 0);

            // Act
            var actualCustId = criteria.CustId;

            // Assert
            Assert.Equal(expectedCustId, actualCustId);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("Dev", "Dev")]
        public void UserName_Always_ReturnUserNameSetByCtor(string userName, string expectedUserName)
        {
            // Arrange
            var criteria = new GetTransferToDownlineHistoryCriteria(0, userName, new DateTime(), new DateTime(), 0, 0);

            // Act
            var actualUserName = criteria.UserName;

            // Assert
            Assert.Equal(expectedUserName, actualUserName);
        }

        [Fact]
        public void FromDate_Always_ReturnsFromDateSetByCtor()
        {
            // Arrange
            var expectedFromDate = DefaultVariablesTest.CurrentDate;
            var criteria = new GetTransferToDownlineHistoryCriteria(0, string.Empty, expectedFromDate, new DateTime(), 0, 0);

            // Act
            var actualFromDate = criteria.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void ToDate_Always_ReturnsToDateSetByCtor()
        {
            // Arrange
            var expectedToDate = DefaultVariablesTest.CurrentDate;
            var criteria = new GetTransferToDownlineHistoryCriteria(0, string.Empty, new DateTime(), expectedToDate, 0, 0);

            // Act
            var actualToDate = criteria.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }

        [Fact]
        public void PageSize_Always_ReturnPageSizeSetByCtor()
        {
            // Arrange
            var expectedPageSize = DefaultVariablesTest.PageSize;
            var criteria = new GetTransferToDownlineHistoryCriteria(0, string.Empty, new DateTime(), new DateTime(), expectedPageSize, 0);

            // Act
            var actualPageSize = criteria.PageSize;

            // Assert
            Assert.Equal(expectedPageSize, actualPageSize);
        }

        [Fact]
        public void PageIndex_Always_ReturnPageIndexSetByCtor()
        {
            // Arrange
            var expectedPageIndex = DefaultVariablesTest.PageIndex;
            var criteria = new GetTransferToDownlineHistoryCriteria(0, string.Empty, new DateTime(), new DateTime(), 0, expectedPageIndex);

            // Act
            var actualPageIndex = criteria.PageIndex;

            // Assert
            Assert.Equal(expectedPageIndex, actualPageIndex);
        }
    }
}