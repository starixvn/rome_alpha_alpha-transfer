using Alpha.Transfer.Criteria.AutoTransfer;

namespace Alpha.Transfer.Tests.Criteria.AutoTransfer
{
    using Xunit;

    [Trait("Category", "CancelJobCriteria")]
    public class CancelJobCriteriaTests
    {
        [Fact]
        public void CancelJobCriteria_Always_ReturnsValidSettingKey()
        {

            // Arrange
            var expectedSettingKey = "Age_ClearBalanceCust_Cancel";
            var criteria = new CancelJobCriteria(1176070, 1176083, null);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new CancelJobCriteria(1176070, 1176083, null);
            bool expectedResult = true;

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualIsValid);
        }

        [Theory]
        [InlineData(-1176070, 1176083)]
        [InlineData(1176070, -1176083)]
        [InlineData(1176070, 1176083)]

        public void IsInvalid_Always_ReturnsFalse(int uplineId, int custId)
        {
            // Arrange
            var criteria = new CancelJobCriteria(uplineId, custId, null);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(false, actualIsValid);
        }
    }
}
