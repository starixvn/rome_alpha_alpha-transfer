namespace Alpha.Transfer.Tests.Criteria.CashBalanceTransfer
{
    using Transfer.Criteria.CashBalanceTransfer;
    using Xunit;

    [Trait("Category", "CashBalanceTransfer")]
    public class CancelJobCriteriaTests
    {
        [Fact]
        public void AddCashBalanceTransferCriteria_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var expectedSettingKey = "SiteDB_Age_Transfer_CashBalance_Ins";
            var criteria = new AddCashBalanceTransferCriteria(1176070, 1176083, 1101240.32M, 1176070, 0, "127.0.0.1", "E");

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new AddCashBalanceTransferCriteria(1176070, 1176083, 1101240.32M, 1176070, 0, "127.0.0.1", "E");
            bool expectedResult = true;

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualIsValid);
        }

        [Theory]
        [InlineData(-1176070, 1176083, 1101240.32, "127.0.0.1", false)]
        [InlineData(1176070, -1176083, 1101240.32, "127.0.0.1", false)]
        [InlineData(1176070, 1176083, 1101240.32, null, false)]
        [InlineData(1176070, 1176083, 0, "127.0.0.1", false)]
        public void IsInvalid_Always_ReturnsFalse(int uplineId, int custId, decimal amount, string ip, bool expectedResult)
        {
            // Arrange
            var criteria = new AddCashBalanceTransferCriteria(uplineId, custId, amount, uplineId, 0, ip, "E");

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualIsValid);
        }
    }
}
