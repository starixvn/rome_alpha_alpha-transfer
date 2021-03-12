namespace Alpha.Transfer.Tests.Criteria.TransferToDownlineHistory
{
    using Alpha.Transfer.Criteria.TransferToDownlineHistory;
    using System;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class GetTransferToDownlineHistoryCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var expectedSettingKey = "Age_Rpt_Downline_Transfer_History";
            var criteria = new GetTransferToDownlineHistoryCriteria(
                0, 
                string.Empty, 
                new DateTime(), 
                new DateTime(), 
                0, 
                0);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        public void IsValid_CustId_ReturnsCorrectValue(int custId, bool expectedValue)
        {
            // Arrange
            var criteria = new GetTransferToDownlineHistoryCriteria(
                custId, 
                string.Empty, 
                new DateTime(),
                new DateTime(), 
                0, 
                0);

            // Act
            var actualValue = criteria.IsValid();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}