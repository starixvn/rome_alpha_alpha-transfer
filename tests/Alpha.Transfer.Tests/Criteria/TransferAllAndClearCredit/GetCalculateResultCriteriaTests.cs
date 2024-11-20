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
            var expectedSettingKey = "SiteDB_Age_Transfer_Credit_All_Sel";
            var criteria = new GetCalculateResultCriteria(1, string.Empty, string.Empty, false, "<ROOT></ROOT>", 0);

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
            var criteria = new GetCalculateResultCriteria(1, username, string.Empty, false, "<ROOT></ROOT>", 1);

            // Act
            var actualValue = criteria.IsValid();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void DownlineNetPosition_Always_CorrectXMLData()
        {
            // Arrange            
            var criteria = new GetCalculateResultCriteria(1, string.Empty, string.Empty, false, "<ROOT></ROOT>", 0);

            // Act
            var actualSettingKey = criteria.DownlineNetPosition;

            // Assert
            Assert.True(actualSettingKey.Contains("<ROOT>"));
            Assert.True(actualSettingKey.Contains("</ROOT>"));
        }
    }
}
