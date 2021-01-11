namespace Alpha.Transfer.Site.Tests.ViewModels.Components
{
    using Alpha.Transfer.Site.ViewModels.Components;
    using Xunit;

    [Trait("Category", "Site.Components")]
    public class PagerViewModelTests
    {
        [Fact]
        public void PageIndex_Always_ReturnsPageIndexSetByCtor()
        {
            // Arrange
            int pageIndex = 0;
            var pagerViewModel = new PagerViewModel(pageIndex, 0, 0);

            // Act
            int actualPageIndex = pagerViewModel.PageIndex;

            // Assert
            Assert.Equal(pageIndex, actualPageIndex);
        }

        [Fact]
        public void PageSize_Always_ReturnsPageSizeSetByCtor()
        {
            // Arrange
            int pageSize = 0;
            var pagerViewModel = new PagerViewModel(0, pageSize, 0);

            // Act
            int actualPageSize = pagerViewModel.PageSize;

            // Assert
            Assert.Equal(pageSize, actualPageSize);
        }

        [Fact]
        public void TotalRecords_Always_ReturnsTotalRecordsSetByCtor()
        {
            // Arrange
            int totalRecords = 0;
            var pagerViewModel = new PagerViewModel(0, 0, totalRecords);

            // Act
            int actualTotalRecords = pagerViewModel.TotalRecords;

            // Assert
            Assert.Equal(totalRecords, actualTotalRecords);
        }
    }
}