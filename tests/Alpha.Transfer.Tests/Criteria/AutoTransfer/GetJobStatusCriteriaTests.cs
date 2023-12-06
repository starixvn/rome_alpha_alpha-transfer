namespace Alpha.Transfer.Tests.Criteria.AutoTransfer
{
    using Alpha.Transfer.Criteria.AutoTransfer;
    using Xunit;

    [Trait("Category", "CancelJobCriteria")]
    public class GetJobStatusCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var expectedSettingKey = "SiteDB_Age_ClearBalanceCust_Job_StatusCheck";
            var criteria = new GetJobStatusCriteria(1, 2, true);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetJobStatusCriteria(1, 2, true);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
