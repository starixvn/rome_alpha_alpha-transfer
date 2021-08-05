namespace Alpha.Transfer.Tests.Criteria.MultipleTransfer
{
    using Alpha.Transfer.Params.MultipleTransfer;
    using System.Collections;
    using Transfer.Criteria.MultipleTransfer;
    using Transfer.Models;
    using Xunit;

    [Trait("Category", "MultipleTransfer")]
    public class GetCustomerListCriteriaTests
    {
        [Theory]
        [InlineData(2, "custid")]
        [InlineData(3, "masterid")]
        [InlineData(4, "sagid")]
        public void GetSpParams_ByUserLevel_ReturnCorectValue(byte userLevel, string expectParam)
        {
            // Arrange
            var param = new GetCustomerListParam(Enumeration.FromValue<UserLevel>(userLevel), 1, string.Empty, 1, 1, false, false, 1);
            var criteria = new GetCustomerListCriteria(param);

            // Act
            var actualValue = criteria.GetSpParams();

            // Assert
            Assert.True(actualValue.ContainsKey(expectParam) && (int)actualValue[expectParam] == 1);
        }

        [Theory]
        [InlineData(1, "custstatus", "Open")]
        [InlineData(2, "custstatus", "Suspended")]
        [InlineData(3, "custstatus", "Closed")]
        [InlineData(4, "custstatus", "Disabled")]
        public void GetSpParams_ByStatus_ReturnCorectValue(int status, string expectParam, string expectValue)
        {
            // Arrange
            var param = new GetCustomerListParam(UserLevel.Agent, 1, string.Empty, 1, 1, false, false, status);
            var criteria = new GetCustomerListCriteria(param);

            // Act
            var actualValue = criteria.GetSpParams();

            // Assert
            Assert.True(actualValue.ContainsKey(expectParam) && (string)actualValue[expectParam] == expectValue);
        }

        [Fact]
        public void GetSpParams_Always_ReturnCorectValue()
        {
            // Arrange
            var param = new GetCustomerListParam(UserLevel.Agent, 1, string.Empty, 1, 1, false, false, 1);
            var expectedValue = new Hashtable
            {
                ["pageindex"] = 1,
                ["pagesize"] = 1,
                ["username"] = string.Empty,
                ["isyesterdaybalance"] = false,
                ["isallowtotalbalance"] = false,
                ["totalpages"] = 0,
                ["custid"] = 1,
                ["custstatus"] = "Open"
            };

            var criteria = new GetCustomerListCriteria(param);

            // Act
            var actualValue = criteria.GetSpParams();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(2, "Age_Transfer_SelList_Ag")]
        [InlineData(3, "Age_Transfer_SelList_MAg")]
        [InlineData(4, "Age_Transfer_SelList_SAg")]
        public void GetSettingKey_Always_ReturnCorectValue(byte userLevel, string expectValue)
        {
            // Arrange
            var param = new GetCustomerListParam(Enumeration.FromValue<UserLevel>(userLevel), 1, string.Empty, 1, 1, false, false, 1);
            var criteria = new GetCustomerListCriteria(param);

            // Act
            var actualValue = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectValue, actualValue);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void IsValid_Always_ReturnCorectValue(int custId, bool expectValue)
        {
            // Arrange
            var param = new GetCustomerListParam(UserLevel.Agent, custId, string.Empty, 1, 1, false, false, 1);
            var criteria = new GetCustomerListCriteria(param);

            // Act
            var actualValue = criteria.IsValid();

            // Assert
            Assert.Equal(expectValue, actualValue);
        }
    }
}