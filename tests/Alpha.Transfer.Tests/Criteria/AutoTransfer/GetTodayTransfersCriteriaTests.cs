namespace Alpha.Transfer.Tests.Criteria.AutoTransfer
{
    using Alpha.Transfer.Criteria.AutoTransfer;
    using Xunit;

    [Trait("Category", "CancelJobCriteria")]
    public class GetTodayTransfersCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var expectedSettingKey = "Age_ClearBalanceCust_ProcessSel";
            var criteria = new GetTodayTransfersCriteria();

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetTodayTransfersCriteria();

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
