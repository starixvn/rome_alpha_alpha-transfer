namespace Alpha.Transfer.Tests.Criteria.CashBalanceTransfer
{
    using Transfer.Criteria.CashBalanceTransfer;
    using Xunit;

    [Trait("Category", "CashBalanceTransfer")]
    public class GetCashBalanceTransferCriteriaTests
    {
        [Fact]
        public void GetCashBalanceTransferCriteria_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var expectedSettingKey = "SiteDB_Age_Transfer_CashBalance_Sel";
            var criteria = new GetCashBalanceTransferCriteria(1176083, false);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_CustId_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetCashBalanceTransferCriteria(1176083, false);
            bool expectedResult = true;

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualIsValid);
        }

        [Theory]
        [InlineData(-1176083, false)]
        [InlineData(0, false)]
        public void IsInvalid_CustId_ReturnsFalse(int custId, bool expectedResult)
        {
            // Arrange
            var criteria = new GetCashBalanceTransferCriteria(custId, false);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualIsValid);
        }
    }
}
