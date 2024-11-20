namespace Alpha.Transfer.Tests.Criteria.TransferAllAndClearCredit
{
    using Transfer.Criteria.TransferAllAndClearCredit;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    public class TransferAllCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var expectedSettingKey = "SiteDB_Age_Transfer_All_Ins";
            var criteria = new TransferAllCriteria(1, 1, 1, string.Empty, string.Empty, string.Empty, false);

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
            var criteria = new TransferAllCriteria(1, 1, 1, username, string.Empty, string.Empty, false);

            // Act
            var actualValue = criteria.IsValid();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
