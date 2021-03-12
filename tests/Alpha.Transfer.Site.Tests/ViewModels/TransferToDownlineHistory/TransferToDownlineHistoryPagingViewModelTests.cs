namespace Alpha.Transfer.Site.Tests.ViewModels.TransferToDownlineHistory
{
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Alpha.Transfer.Site.ViewModels.TransferToDownlineHistory;
    using KellermanSoftware.CompareNetObjects;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class TransferToDownlineHistoryPagingViewModelTests
    {
        private readonly CompareLogic compareLogic;

        public TransferToDownlineHistoryPagingViewModelTests()
        {
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void TransferToDownlines_Always_ReturnCorrectValuesSetByCtor()
        {
            // Arrange
            var expectedTransferToDownlines = new List<TransferToDownlineHistoryModel>
            {
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 11), "Agent", 0, 1, 2, 3, "Desc 1"),
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 12), "Agent", 1, 2, 3, 4, string.Empty),
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 13), "Agent", 0, 3, 4, 5, "Desc 3")
            };
            var viewModel = new TransferToDownlineHistoryPagingViewModel(expectedTransferToDownlines, 500);

            // Act
            var actualTransferToDownlines = viewModel.TransferToDownlines;

            // Assert
            Assert.True(compareLogic.Compare(expectedTransferToDownlines.ElementAt(0), actualTransferToDownlines.ElementAt(0)).AreEqual
                && compareLogic.Compare(expectedTransferToDownlines.ElementAt(1), actualTransferToDownlines.ElementAt(1)).AreEqual
                && compareLogic.Compare(expectedTransferToDownlines.ElementAt(2), actualTransferToDownlines.ElementAt(2)).AreEqual);
        }

        [Fact]
        public void TotalRecords_Always_ReturnCorrectValueSetByCtor()
        {
            // Arrange
            var expectedTotalRecords = 500;
            var viewModel = new TransferToDownlineHistoryPagingViewModel(new List<TransferToDownlineHistoryModel>(), expectedTotalRecords);

            // Act
            var actualTotalRecords = viewModel.TotalRecords;

            // Arrange
            Assert.Equal(expectedTotalRecords, actualTotalRecords);
        }
    }
}