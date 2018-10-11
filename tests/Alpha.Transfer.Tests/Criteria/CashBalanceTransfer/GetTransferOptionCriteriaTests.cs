namespace Alpha.Transfer.Tests.Criteria.CashBalanceTransfer
{
    using Transfer.Criteria;
    using Xunit;

    [Trait("Category", "Transfer")]
    public class GetTransferOptionCriteriaTests
    {
        [Fact]
        public void GetTransferOptionCriteria_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var expectedSettingKey = "Acc_CustomerSelTransferOption";
            var criteria = new GetTransferOptionCriteria(1176083);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_CustId_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetTransferOptionCriteria(1176083);
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
            var criteria = new GetTransferOptionCriteria(custId);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualIsValid);
        }
    }
}
