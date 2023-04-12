using Alpha.Transfer.Criteria.AutoTransfer;

namespace Alpha.Transfer.Tests.Criteria.AutoTransfer
{
    using Alpha.Transfer.Params.AutoTransfer;
    using Xunit;

    [Trait("Category", "CancelJobCriteria")]
    public class CancelJobCriteriaTests
    {
        [Fact]
        public void CancelJobCriteria_Always_ReturnsValidSettingKey()
        {

            // Arrange
            var expectedSettingKey = "Age_ClearBalanceCust_Cancel";
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

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var param = new CancelJobParam
            {
                FromCustId = 1176083,
                ToCustId = 1176070
            };

            var criteria = new CancelJobCriteria(param);
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
            var param = new CancelJobParam
            {
                FromCustId = uplineId,
                ToCustId = custId
            };

            var criteria = new CancelJobCriteria(param);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(false, actualIsValid);
        }
    }
}
