namespace Alpha.Transfer.Tests.Extensions
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Alpha.Transfer.Extensions;
    using Xunit;

    [Trait("Category", "Extensions")]
    public class IEnumerableExtensionsTests
    {
        [Fact]
        public void GetSelectListItems_Always_ReturnsCorrectValue()
        {
            // Arrange
            var columns = new[] { "Id" };
            var expectedValue = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Id" }
            };

            // Act
            var actualValue = columns.GetSelectListItems();

            // Assert
            AssertExtension.Equal(expectedValue, actualValue);
        }
    }
}