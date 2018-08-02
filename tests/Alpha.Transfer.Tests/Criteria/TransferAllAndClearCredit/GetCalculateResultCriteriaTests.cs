namespace Alpha.Transfer.Tests.Criteria.TransferAllAndClearCredit
{
    using Transfer.Criteria.TransferAllAndClearCredit;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    public class GetCalculateResultCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var expectedSettingKey = "Age_Transfer_Credit_All_Sel";
            var criteria = new GetCalculateResultCriteria(1, string.Empty);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("  ", false)]
        [InlineData("username", true)]
        public void IsValid_CustId_ReturnsTrue(string username, bool expectedValue)
        {
            // Arrange
            var criteria = new GetCalculateResultCriteria(1, username);

            // Act
            var actualValue = criteria.IsValid();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
