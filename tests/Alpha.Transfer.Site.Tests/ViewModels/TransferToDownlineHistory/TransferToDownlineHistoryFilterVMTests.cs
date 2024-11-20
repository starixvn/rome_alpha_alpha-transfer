namespace Alpha.Transfer.Site.Tests.ViewModels.TransferToDownlineHistory
{
    using Alpha.Transfer.AppResources;
    using Alpha.Transfer.Extensions;
    using Alpha.Transfer.Models;
    using Alpha.Transfer.Site.ViewModels.Components;
    using Alpha.Transfer.Site.ViewModels.TransferToDownlineHistory;
    using KellermanSoftware.CompareNetObjects;
    using System;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class TransferToDownlineHistoryFilterVMTests
    {
        private readonly CompareLogic compareLogic;

        public TransferToDownlineHistoryFilterVMTests()
        {
            compareLogic = new CompareLogic();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("username", "username")]
        public void UserName_Always_ReturnsUserNameSetByCtor(string userName, string expectedUserName)
        {
            // Arrange
            var filterViewModel = new TransferToDownlineHistoryFilterViewModel(userName, null, new DateTime(), UserLevel.Agent);

            // Act
            var actualUserName = filterViewModel.UserName;

            // Assert
            Assert.Equal(expectedUserName, actualUserName);
        }

        [Fact]
        public void DateRangePickerViewModel_Always_ReturnsValueSetByCtor()
        {
            // Arrange
            var dateRangePickerVm = new DateRangePickerViewModel(DateTime.MinValue, DateTime.MaxValue, new DateTime(), new DateTime(), 0);
            var filterViewModel = new TransferToDownlineHistoryFilterViewModel(string.Empty, dateRangePickerVm, new DateTime(), UserLevel.Agent);

            // Act
            var actualDateRangePickerVm = filterViewModel.DateRangePickerViewModel;

            // Assert
            Assert.True(compareLogic.Compare(dateRangePickerVm, actualDateRangePickerVm).AreEqual);
        }

        [Fact]
        public void ViewDataInstructionMessage_Always_ReturnsCorrectValueWithMinDate()
        {
            // Arrange
            var minDate = new DateTime();
            var expectedMessage = string.Format(Resources.CanViewReportDataFrom, minDate.ToShortDateWithSlash());
            var filterViewModel = new TransferToDownlineHistoryFilterViewModel(string.Empty, null, minDate, UserLevel.Agent);

            // Act
            var actualMessage = filterViewModel.ViewDataInstructionMessage;

            // Arrange
            Assert.Equal(expectedMessage, actualMessage);
        }
    }
}