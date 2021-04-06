namespace Alpha.Transfer.Site.Tests.ViewModels.Components
{
    using Alpha.Transfer.AppResources;
    using Alpha.Transfer.Site.ViewModels.Components;
    using System;
    using System.Linq;
    using Xunit;

    [Trait("Category", "Site.Components")]
    public class DateRangePickerViewModelTests
    {
        [Fact]
        public void Constructor_Always_InitQuickChooseOptions()
        {
            // Act
            var viewModel = new DateRangePickerViewModel(DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, 1);

            // Assert
            Assert.True(viewModel.RangeOptions.ElementAt(0).Value == "0"
                    && viewModel.RangeOptions.ElementAt(0).Text == Resources.Today
                    && viewModel.RangeOptions.ElementAt(1).Value == "1"
                    && viewModel.RangeOptions.ElementAt(1).Text == Resources.Yesterday
                    && viewModel.RangeOptions.ElementAt(2).Value == "2"
                    && viewModel.RangeOptions.ElementAt(2).Text == Resources.ThisWeek
                    && viewModel.RangeOptions.ElementAt(3).Value == "3"
                    && viewModel.RangeOptions.ElementAt(3).Text == Resources.LastWeek
                    && viewModel.RangeOptions.ElementAt(4).Value == "4"
                    && viewModel.RangeOptions.ElementAt(4).Text == Resources.ThisMonth
                    && viewModel.RangeOptions.ElementAt(5).Value == "5"
                    && viewModel.RangeOptions.ElementAt(5).Text == Resources.LastMonth
                    && viewModel.RangeOptions.ElementAt(6).Value == "6"
                    && viewModel.RangeOptions.ElementAt(6).Text == Resources.SinceLastMonth);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void Constructor_UserSelectsQuickChooseOption_ReturnSelectedOption(int quickChooseOption)
        {
            // Act
            var viewModel = new DateRangePickerViewModel(DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, quickChooseOption);

            // Assert
            Assert.True(viewModel.RangeOptions.ElementAt(quickChooseOption).Selected);
        }

        [Fact]
        public void Constructor_Always_SetDates()
        {
            // Arrange
            var minDate = new DateTime(1989, 5, 28);
            var maxDate = new DateTime(1989, 4, 28);
            var fromDate = new DateTime(1989, 3, 28);
            var toDate = new DateTime(1989, 2, 28);

            // Act
            var viewModel = new DateRangePickerViewModel(minDate, maxDate, fromDate, toDate, 1);

            // Assert
            Assert.True(viewModel.MinDate == minDate
                    && viewModel.MaxDate == maxDate
                    && viewModel.FromDate == fromDate
                    && viewModel.ToDate == toDate);
        }

        [Fact]
        public void Constructor_NotSetOptionForIsRenderSubmitButton_SetDefaultRender()
        {
            // Act
            var viewModel = new DateRangePickerViewModel(DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, 1);

            // Assert
            Assert.True(viewModel.IsRenderSubmitButton);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Constructor_Always_SetRenderingSubmitButton(bool isRenderSubmitButton)
        {
            // Act
            var viewModel = new DateRangePickerViewModel(DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, 1, isRenderSubmitButton);

            // Assert
            Assert.Equal(isRenderSubmitButton, viewModel.IsRenderSubmitButton);
        }
    }
}