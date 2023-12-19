namespace Alpha.Transfer.Tests.Criteria.AutoTransfer
{
    using Alpha.Transfer.Criteria.AutoTransfer;
    using Xunit;

    [Trait("Category", "CancelJobCriteria")]
    public class GetHistoryTransfersCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var expectedSettingKey = "Age_ClearBalanceCust_ProcessHistorySel";
            var criteria = new GetHistoryTransfersCriteria();

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetHistoryTransfersCriteria { ViewStatus = "All" };

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
