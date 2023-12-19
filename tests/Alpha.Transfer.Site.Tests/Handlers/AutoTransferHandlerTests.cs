using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Transfer.AppResources;
using Alpha.Transfer.Constants;
using Alpha.Transfer.Models;
using Alpha.Transfer.Models.AutoTransfer;
using Alpha.Transfer.Params.AutoTransfer;
using Alpha.Transfer.Services;
using Alpha.Transfer.Site.Handlers;
using Alpha.Transfer.Site.Requests.AutoTransfer;
using Alpha.Transfer.Site.Tests.Extensions;
using Alpha.Transfer.Site.ViewModels.AutoTransfer;
using NSubstitute;
using Xunit;

namespace Alpha.Transfer.Site.Tests.Handlers
{
    [Trait("Category", "AutoTransfer")]
    public class AutoTransferHandlerTests
    {
        public enum Status
        {
            All,
            New,
            Waiting,
            Processing,
            Completed,
            Failed
        }

        private readonly IUser user;
        private readonly AutoTransferService autoTransferService;
        private readonly CashBalanceTransferService cashBalanceTransferService;
        private readonly AutoTransferHandler handler;

        public AutoTransferHandlerTests()
        {
            user = Substitute.For<IUser>();
            user.Id.Returns(1);
            autoTransferService = Substitute.For<AutoTransferService>();
            cashBalanceTransferService = Substitute.For<CashBalanceTransferService>();
            handler = new AutoTransferHandler(autoTransferService, cashBalanceTransferService);
        }

        [Fact]
        public async Task HandlerGetTransferCondition_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            var getTransferConditionQuery = new GetTransferConditionQuery
            {
                UplineId = user.Id,
                ViewerLevel = user.LevelId,
                SearchAccountIds = "1176083,33278659,14373487",
                Usernames = "master1, master1p1, masterkz"
            };

            cashBalanceTransferService.GetTransferCondition(Arg.Any<int>()).Returns(new Tuple<bool, IEnumerable<string>>(false, Enumerable.Empty<string>()));

            var expectedViewModel = new GetTransferConditionViewModel
            {
                SearchAccountIds = getTransferConditionQuery.SearchAccountIds,
                Usernames = getTransferConditionQuery.Usernames,
                TransferCondition = string.Empty
            };

            // Act
            var actualViewModel = await handler.Handle(getTransferConditionQuery);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Theory]
        [InlineData("On")]
        [InlineData("Off")]
        public async Task HandlerUpdateQuery_UpdateSuccess_ReturnsCorrectResult(string state)
        {
            // Arrange
            user.Id.Returns(1176070);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");

            var message = new UpdateQuery(user)
            {
                CustId = 1176070,
                State = state.ToLowerInvariant()
            };

            var result = new Result()
            {
                Code = 0
            };
            autoTransferService.UpdateAsync(Arg.Any<UpdateSingleParam>()).Returns(result);

            var expectedViewModel = new Result
            {
                Code = 0,
                Message = string.Format(Resources.ResourceManager.GetString("AutoTransfer_UpdateSuccess" + state), user.Name)
            };

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public async Task HandlerUpdateQuery_UpdateFailed_ReturnsCorrectResult()
        {
            // Arrange
            user.Id.Returns(1176070);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");

            var message = new UpdateQuery(user)
            {
                CustId = 1176070,
                State = AutoTransferConstant.On
            };

            var result = new Result()
            {
                Code = 1,
                Message = "Error"
            };

            autoTransferService.UpdateAsync(Arg.Any<UpdateSingleParam>()).Returns(result);

            var expectedViewModel = new Result
            {
                Code = 1,
                Message = "Error"
            };

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Theory]
        [InlineData(AutoTransferConstant.All)]
        [InlineData(AutoTransferConstant.New)]
        [InlineData(AutoTransferConstant.Waiting)]
        [InlineData(AutoTransferConstant.Processing)]
        [InlineData(AutoTransferConstant.Completed)]
        [InlineData(AutoTransferConstant.Failed)]
        public async Task HandlerGetTodayTransfersQuery_Always_ReturnsCorrectViewModel(string status)
        {
            // Arrange
            user.Id.Returns(1176070);
            var message = new GetTodayTransfersQuery(user)
            {
                Status = status,
                PageIndex = 1,
                PageSize = 20,
                SortedBy = 0,
                Direction = 1
            };

            var param = new GetTodayTransfersParam
            {
                CustId = message.CustId,
                Username = message.Username,
                Status = message.Status,
                PageSize = message.PageSize,
                PageIndex = message.PageIndex,
                SortedBy = message.SortedBy,
                Direction = message.Direction
            };

            var expectedViewModel = new AutoTransferMonitoringPageModel();
            autoTransferService.GetTodayTransfersAsync(Arg.Any<GetTodayTransfersParam>()).Returns(expectedViewModel);

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Theory]
        [InlineData(AutoTransferConstant.All)]
        [InlineData(AutoTransferConstant.New)]
        [InlineData(AutoTransferConstant.Waiting)]
        [InlineData(AutoTransferConstant.Processing)]
        [InlineData(AutoTransferConstant.Completed)]
        [InlineData(AutoTransferConstant.Failed)]
        public async Task HandlerGetHistoryTransfersQuery_StatusIsAll_ReturnsCorrectViewModel(string status)
        {
            // Arrange
            user.Id.Returns(1176070);
            var message = new GetHistoryTransfersQuery(user)
            {
                Status = status,
                PageIndex = 1,
                PageSize = 20,
                SortedBy = 0,
                Direction = 1
            };

            var param = new GetHistoryTransfersParam
            {
                CustId = message.CustId,
                Username = message.Username,
                Status = message.Status,
                PageSize = message.PageSize,
                PageIndex = message.PageIndex,
                SortedBy = message.SortedBy,
                Direction = message.Direction,
                FromDate = message.FromDate,
                ToDate = message.ToDate
            };

            var expectedViewModel = new AutoTransferMonitoringPageModel();
            autoTransferService.GetHistoryTransfersAsync(Arg.Any<GetHistoryTransfersParam>()).Returns(expectedViewModel);

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public async Task HandlerCancelJobQuery_Always_ReturnsCorrectResult()
        {
            // Arrange
            user.Id.Returns(1176070);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");

            var message = new CancelJobQuery(user)
            {
                CustId = 1176070,
            };

            var expectedViewModel = new Result
            {
                Code = 0,
                Message = Resources.AutoTransfer_CancelSuccess
            };

            autoTransferService.CancelJobAsync(Arg.Any<CancelJobParam>()).Returns(expectedViewModel);

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }
    }
}
