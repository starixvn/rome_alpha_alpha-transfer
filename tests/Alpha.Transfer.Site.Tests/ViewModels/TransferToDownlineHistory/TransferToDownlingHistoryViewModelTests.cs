namespace Alpha.Transfer.Site.Tests.ViewModels.TransferToDownlineHistory
{
    using Alpha.Transfer.Models;
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Alpha.Transfer.Site.Requests.TransferToDownlineHistory;
    using Alpha.Transfer.Site.ViewModels.Components;
    using Alpha.Transfer.Site.ViewModels.TransferToDownlineHistory;
    using Alpha.Transfer.Utils;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class TransferToDownlingHistoryViewModelTests
    {
        private const string userName = "UserName";

        private const int userSelectDateRangeOption = 0;
        private const int totalRecords = 10;
        private const int pageSize = 500;
        private const int page = 1;

        private readonly DateTime today = new DateTime(2021, 1, 7);
        private readonly DateTime fromDate = new DateTime(2021, 1, 11);
        private readonly DateTime toDate = new DateTime(2021, 3, 11);
        private readonly DateTime minDate = new DateTime(2020, 11, 7);
        private readonly DateTime maxDate = DateTime.MaxValue;

        private readonly IUser user;
        private readonly IEnumerable<TransferToDownlineHistoryModel> records;
        private readonly TransferToDownlineHistoryQuery query;
        private readonly TransferToDownlineHistoryParam param;
        private readonly TransferToDownlineHistoryViewModel viewModel;
        private readonly CompareLogic compareLogic;

        public TransferToDownlingHistoryViewModelTests()
        {
            user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);

            query = new TransferToDownlineHistoryQuery(user)
            {
                UserName = userName,
                UserSelectedDateRangeOption = userSelectDateRangeOption
            };
            param = new TransferToDownlineHistoryParam(0, string.Empty, fromDate, toDate);
            records = new List<TransferToDownlineHistoryModel>
            {
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 11), "Agent", 0, 1, 2, 3, "Desc 1"),
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 12), "Agent", 1, 2, 3, 4, string.Empty),
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 13), "Agent", 0, 3, 4, 5, "Desc 3")
            };
            viewModel = new TransferToDownlineHistoryViewModel(query, param);

            compareLogic = new CompareLogic();

            SystemTime.Today = today;
        }

        [Fact]
        public void UserName_Always_ReturnsUserNameFromQuery()
        {
            // Act
            var actualUserName = viewModel.UserName;

            // Assert
            Assert.Equal(userName, actualUserName);
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        [InlineData(2, "Member")]
        public void DirectDownlineText_Always_ReturnCorrectValue(byte level, string expectedValue)
        {
            // Arrange
            var message = new TransferToDownlineHistoryQuery(user)
            {
                CustLevelId = level
            };

            var vm = new TransferToDownlineHistoryViewModel(message, null);

            // Act
            var actualDirectDownlineText = vm.DirectDownlineText;

            // Assert
            Assert.Equal(expectedValue, actualDirectDownlineText);
        }

        [Fact]
        public void MinDate_Always_ReturnsCorrectValue()
        {
            // Act
            var actualMinDate = TransferToDownlineHistoryViewModel.MinDate;

            // Assert
            Assert.Equal(minDate, actualMinDate);
        }

        [Fact]
        public void MaxDate_Always_ReturnsCorrectValue()
        {
            // Act
            var actualMaxDate = TransferToDownlineHistoryViewModel.MaxDate;

            // Assert
            Assert.Equal(maxDate, actualMaxDate);
        }

        [Fact]
        public void FromDate_Always_ReturnsFromDateFromParam()
        {
            // Act
            var actualFromDate = viewModel.FromDate;

            // Assert
            Assert.Equal(fromDate, actualFromDate);
        }

        [Fact]
        public void ToDate_Always_ReturnsToDateFromParam()
        {
            // Act
            var actualToDate = viewModel.ToDate;

            // Assert
            Assert.Equal(toDate, actualToDate);
        }

        [Fact]
        public void UserSelectedDateRangeOption_Always_ReturnsValueFromQuery()
        {
            // Act
            var actualUserSelectedDateRangeOption = viewModel.UserSelectedDateRangeOption;

            // Assert
            Assert.Equal(userSelectDateRangeOption, actualUserSelectedDateRangeOption);
        }

        [Fact]
        public void FilterViewModel_Always_ReturnCorrectValue()
        {
            // Arrange
            var dateRangePickerVM = new DateRangePickerViewModel(minDate, maxDate, fromDate, toDate, userSelectDateRangeOption);
            var filterVM = new TransferToDownlineHistoryFilterViewModel(userName, dateRangePickerVM, minDate);

            // Act
            var actualFilterVM = viewModel.FilterViewModel;

            // Assert
            Assert.True(compareLogic.Compare(filterVM, actualFilterVM).AreEqual);
        }
    }
}