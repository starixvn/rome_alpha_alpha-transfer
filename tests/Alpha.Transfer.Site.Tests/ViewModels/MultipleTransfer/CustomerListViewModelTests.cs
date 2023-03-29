namespace Alpha.Transfer.Site.Tests.ViewModels.MultipleTransfer
{
    using System.Linq;
    using NSubstitute;
    using Requests.MultipleTransfer;
    using Site.ViewModels.MultipleTransfer;
    using Transfer.Models;
    using Transfer.Models.MultipleTransfer;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    public class CustomerListViewModelTests
    {
        private readonly IUser user;
        private readonly CustomerListQuery query;
        private readonly CustomerListModel model;
        private readonly CustomerListViewModel customerListViewModel;

        public CustomerListViewModelTests()
        {
            user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);

            query = new CustomerListQuery(user)
            {
                Username = "username"
            };

            model = new CustomerListModel(Enumerable.Empty<CustomerInformation>(), 3);
            customerListViewModel = new CustomerListViewModel(query, model, false);
        }

        [Theory]
        [InlineData(-1, "favorite")]
        [InlineData(0, "")]
        [InlineData(1, "")]
        public void GetBalanceColor_Always_ReturnCorrectValue(decimal balance, string expectedColor)
        {
            // Act
            var actualValue = customerListViewModel.GetBalanceColor(new CustomerInformation { Balance = balance });

            // Assert
            Assert.Equal(expectedColor, actualValue);
        }

        [Theory]
        [InlineData(-1, "favorite")]
        [InlineData(0, "")]
        [InlineData(1, "")]
        public void GetCashBalanceColor_Always_ReturnCorrectValue(decimal cashBalanceColor, string expectedColor)
        {
            // Act
            var actualValue = customerListViewModel.GetCashBalanceColor(cashBalanceColor);

            // Assert
            Assert.Equal(expectedColor, actualValue);
        }

        [Theory]
        [InlineData(-1, true)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        public void ShowYesterdayBalanceLink_Always_ReturnCorrectValue(decimal yesterdayBalance, bool expectedValue)
        {
            // Act
            var actualValue = customerListViewModel.ShowYesterdayBalanceLink(new CustomerInformation { YesterdayBalance = yesterdayBalance });

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(-1, true)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        public void Balance_Always_ReturnCorrectValue(decimal balance, bool expectedValue)
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.AllowTotalBalanceTransfer.Returns(true);
            user.LevelId.Returns((byte)4);
            var query = new CustomerListQuery(user);
            var customerListViewModel = new CustomerListViewModel(query, model, false);

            // Act
            var actualValue = customerListViewModel.ShowBalanceLink(new CustomerInformation { Balance = balance });

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShowCashBalanceLink_Always_ReturnCorrectValue(bool allowAllDayBalanceTransfer)
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);
            var query = new CustomerListQuery(user);
            var customerListViewModel = new CustomerListViewModel(query, model, allowAllDayBalanceTransfer);

            // Act
            var actualValue = customerListViewModel.ShowCashBalanceLink(new CustomerInformation());

            // Assert
            Assert.Equal(allowAllDayBalanceTransfer, actualValue);
        }

        [Theory]
        [InlineData(true, 0, 0, false)]
        [InlineData(true, 0, 1, false)]
        [InlineData(true, 1, 0, false)]
        [InlineData(true, 1, 1, true)]
        [InlineData(false, 0, 0, false)]
        [InlineData(false, 0, 1, false)]
        [InlineData(false, 1, 0, false)]
        [InlineData(false, 1, 1, false)]
        public void CanTransferYesterday_AllowAllDayBalanceTransfer_ReturnCorrectValue(
            bool allowTotalBalanceTransfer, decimal balance, decimal yesterdayBalance, bool expectedValue)
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);
            user.AllowTotalBalanceTransfer.Returns(allowTotalBalanceTransfer);
            var query = new CustomerListQuery(user);
            var customerListViewModel = new CustomerListViewModel(query, model, true);

            // Act
            var actualValue = customerListViewModel.CanTransferYesterday(new CustomerInformation
            {
                Balance = balance,
                YesterdayBalance = yesterdayBalance
            });

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        public void CanTransferYesterday_AllowTotalBalanceTransfer_ReturnCorrectValue(decimal balance, bool expectedValue)
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);
            user.AllowTotalBalanceTransfer.Returns(true);
            var query = new CustomerListQuery(user);
            var customerListViewModel = new CustomerListViewModel(query, model, false);

            // Act
            var actualValue = customerListViewModel.CanTransferYesterday(new CustomerInformation { Balance = balance });

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        public void CanTransferYesterday_NotAllowTotalBalanceTransfer_ReturnCorrectValue(decimal yesterdayBalance, bool expectedValue)
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);
            user.AllowTotalBalanceTransfer.Returns(false);
            var query = new CustomerListQuery(user);
            var customerListViewModel = new CustomerListViewModel(query, model, false);

            // Act
            var actualValue = customerListViewModel.CanTransferYesterday(new CustomerInformation { YesterdayBalance = yesterdayBalance });

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(0, 1, "")]
        [InlineData(1, 0, "")]
        [InlineData(1, 1, "")]
        public void GetYesterdayBalanceColor_AllowAllDayBalanceTransferAndAllowTotalBalanceTransfer_ReturnCorrectValue(decimal balance, decimal yesterdayBalance, string expectedValue)
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);
            user.AllowTotalBalanceTransfer.Returns(true);
            var query = new CustomerListQuery(user);
            var customerListViewModel = new CustomerListViewModel(query, model, true);

            // Act
            var actualValue = customerListViewModel.GetYesterdayBalanceColor(new CustomerInformation
            {
                Balance = balance,
                YesterdayBalance = yesterdayBalance
            });

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetYesterdayBalanceColor_NotAllowTotalBalanceTransferAndNegativeYesterdayBalance_ReturnCorrectValue()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);
            user.AllowTotalBalanceTransfer.Returns(false);
            var query = new CustomerListQuery(user);
            var customerListViewModel = new CustomerListViewModel(query, model, true);

            // Act
            var actualValue = customerListViewModel.GetYesterdayBalanceColor(new CustomerInformation { YesterdayBalance = -1 });

            // Assert
            Assert.Equal("favorite", actualValue);
        }

        [Fact]
        public void GetYesterdayBalanceColor_NotAllowAllDayBalanceTransferAndAllowTotalBalanceTransfer_ReturnCorrectValue()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);
            user.AllowTotalBalanceTransfer.Returns(true);
            var query = new CustomerListQuery(user);
            var customerListViewModel = new CustomerListViewModel(query, model, false);

            // Act
            var actualValue = customerListViewModel.GetYesterdayBalanceColor(new CustomerInformation { YesterdayBalance = -1 });

            // Assert
            Assert.Equal("", actualValue);
        }

        [Theory]
        [InlineData(0, "underdog")]
        [InlineData(1, "underdog")]
        [InlineData(-1, "favorite")]
        public void GetYesterdayBalanceColor_NotAllowAllDayBalanceTransferAndNotAllowTotalBalanceTransfer_ReturnCorrectValue(
            decimal yesterdayBalance, string expectedValue)
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);
            user.AllowTotalBalanceTransfer.Returns(false);
            var query = new CustomerListQuery(user);
            var customerListViewModel = new CustomerListViewModel(query, model, false);

            // Act
            var actualValue = customerListViewModel.GetYesterdayBalanceColor(new CustomerInformation { YesterdayBalance = yesterdayBalance });

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
