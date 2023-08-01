namespace Alpha.Transfer.Tests.Criteria.TransferAllAndClearCredit
{
    using Alpha.Transfer.Models.TransferAllAndClearCredit;
    using System.Collections.Generic;
    using Transfer.Criteria.TransferAllAndClearCredit;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    public class ClearCreditCriteriaTests
    {
        [Fact]
        public void MemberAmountXML_Always_CorrectXMLData()
        {
            // Arrange            
            var criteria = new ClearCreditCriteria(1, 1, 1, string.Empty, string.Empty, string.Empty, new List<TotalAmtModel>());

            // Act
            var actualSettingKey = criteria.MemberAmountXML;

            // Assert
            Assert.True(actualSettingKey.Contains("<ROOT>"));
            Assert.True(actualSettingKey.Contains("</ROOT>"));
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var expectedSettingKey = "Age_ClearCredit_All_Ins";
            var criteria = new ClearCreditCriteria(1, 1, 1, string.Empty, string.Empty, string.Empty, new List<TotalAmtModel>());

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
            var criteria = new ClearCreditCriteria(1, 1, 1, username, string.Empty, string.Empty, new List<TotalAmtModel>());

            // Act
            var actualValue = criteria.IsValid();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }


    }
}
