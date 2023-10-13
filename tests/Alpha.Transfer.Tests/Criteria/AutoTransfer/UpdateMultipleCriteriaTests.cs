using System.Collections.Generic;
using Alpha.Transfer.Constants;
using Alpha.Transfer.Criteria.AutoTransfer;
using Alpha.Transfer.Params.AutoTransfer;
using NSubstitute;
using Xunit;

namespace Alpha.Transfer.Tests.Criteria.AutoTransfer
{
    [Trait("Category", "UpdateMultipleCriteria")]
    public class UpdateMultipleCriteriaTests
    {
        [Theory]
        [InlineData(AutoTransferConstant.On)]
        [InlineData(AutoTransferConstant.Off)]
        public void GetSettingKey_Always_ReturnsValidSettingKey(string state)
        {
            // Arrange
            var expectedSettingKey = "Age_ClearBalanceCust_SettingUpdate";
            var param = new UpdateStatusParam
            {
                CustId = 1176083,
                UplineId = 1176070,
                IP = "127.0.0.1",
                Language = "E",
                State = state,
                Schedule = new[] { "1" },
            };

            var criteria = new UpdateStatusCriteria(param, true);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(AutoTransferConstant.On)]
        [InlineData(AutoTransferConstant.Off)]
        public void IsValid_ValidCriteria_ReturnsTrue(string state)
        {
            // Arrange
            var param = new UpdateStatusParam
            {
                CustId = 1176083,
                UplineId = 1176070,
                IP = "127.0.0.1",
                Language = "E",
                State = state,
                Schedule = new[] { "1" },
            };

            var criteria = new UpdateStatusCriteria(param, true);
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
        public void IsValid_InvalidCriteria_ReturnsFalse(int fromCustId, int toCustId, string ip)
        {
            // Arrange
            var param = new UpdateStatusParam
            {
                CustId = toCustId,
                UplineId = fromCustId,
                IP = ip,
                Language = "E",
                State = "on",
                Schedule = new[] { "1" },
            };
            var criteria = new UpdateStatusCriteria(param, true);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(false, actualIsValid);
        }
    }
}
