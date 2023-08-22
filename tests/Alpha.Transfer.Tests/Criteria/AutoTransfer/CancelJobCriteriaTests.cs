namespace Alpha.Transfer.Tests.Criteria.AutoTransfer
{
    using Alpha.Transfer.Criteria.AutoTransfer;
    using Alpha.Transfer.Params.AutoTransfer;
    using Xunit;

    [Trait("Category", "CancelJobCriteria")]
    public class CancelJobCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var expectedSettingKey = "Age_ClearBalanceCust_ProcessCancel";
            var param = new CancelJobParam
            {
                FromCustId = 1176070,
                ToCustId = 1176083
            };

            var criteria = new CancelJobCriteria(param);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(-1176070, 0)]
        [InlineData(-1176070, 1)]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void IsValid_InvalidFromCustIdAndToCustId_ReturnsFalse(int fromCustId, int toCustId)
        {
            // Arrange
            var param = new CancelJobParam
            {
                FromCustId = fromCustId,
                ToCustId = toCustId
            };

            var criteria = new CancelJobCriteria(param);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Theory]
        [InlineData(1176070, 1176083)]
        public void IsValid_ValidFromCustIdAndToCustId_ReturnsFalse(int uplineId, int custId)
        {
            // Arrange
            var param = new CancelJobParam
            {
                FromCustId = uplineId,
                ToCustId = custId
            };

            var criteria = new CancelJobCriteria(param);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
