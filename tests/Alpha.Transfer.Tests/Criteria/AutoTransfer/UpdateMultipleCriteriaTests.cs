using Alpha.Transfer.Constants;
using Alpha.Transfer.Criteria.AutoTransfer;
using NSubstitute;

namespace Alpha.Transfer.Tests.Criteria.AutoTransfer
{
    using Xunit;

    [Trait("Category", "UpdateMultipleCriteria")]
    public class UpdateMultipleCriteriaTests
    {
        [Theory]
        [InlineData(AutoTransferConstant.On)]
        [InlineData(AutoTransferConstant.Off)]
        public void UpdateMultipleCriteria_Always_ReturnsValidSettingKey(string state)
        {
            // Arrange
            var expectedSettingKey = "Age_ClearBalanceCust_SettingUpdate";
            var criteria = new UpdateMultipleCriteria(1176070, 1176083, Arg.Any<bool>(), "E", "127.0.0.1", state);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }


        [Theory]
        [InlineData(AutoTransferConstant.On)]
        [InlineData(AutoTransferConstant.Off)]
        public void IsValid_Always_ReturnsTrue(string state)
        {
            // Arrange
            var criteria = new UpdateMultipleCriteria(1176070, 1176083, Arg.Any<bool>(), "E", "127.0.0.1", state);
            bool expectedResult = true;

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, actualIsValid);
        }

        [Theory]
        [InlineData(-1176070, 1176083, "127.0.0.1")]
        [InlineData(1176070, -1176083, "127.0.0.1")]
        [InlineData(1176070, 1176083, null)]

        public void IsInvalid_Always_ReturnsFalse(int fromCustId, int toCustId, string ip)
        {
            // Arrange
            var criteria = new UpdateMultipleCriteria(fromCustId, toCustId, Arg.Any<bool>(), "E", ip, "on");

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(false, actualIsValid);
        }
    }
}
