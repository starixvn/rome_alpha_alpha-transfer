using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Transfer.AppResources;
using Alpha.Transfer.Constants;
using Alpha.Transfer.Models;
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
            var message = new UpdateMultipleQuery(user);
            var getTransferConditionQuery = new GetTransferConditionQuery(message)
            {
                SearchAccountIds = "1176083,33278659,14373487",
                Usernames = "master1, master1p1, masterkz"
            };

            var result = await cashBalanceTransferService.GetTransferCondition(Arg.Any<int>()); ;

            var expectedViewModel = new GetTransferConditionViewModel
            {
                SearchAccountIds = getTransferConditionQuery.SearchAccountIds,
                Usernames = getTransferConditionQuery.Usernames,
                TransferCondition = result
            };

            // Act
            var actualViewModel = await handler.Handle(getTransferConditionQuery);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);


        }

        [Theory]
        [InlineData(AutoTransferConstant.On)]
        [InlineData(AutoTransferConstant.On)]
        [InlineData(AutoTransferConstant.Off)]
        [InlineData(AutoTransferConstant.Off)]
        public async Task HandlerUpdateMultipleQuery_CustIdsIsValid_ReturnsCorrectResults(string state)
        {
            // Arrange
            user.Id.Returns(1176070);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("en-US");

            var message = new UpdateMultipleQuery(user)
            {
                SearchAccountIds = "1176083,33278659,14373883",
                State = state
            };

            var ids = message.SearchAccountIds.Split(new[] { ',' });

            foreach (var id in ids)
            {
                int custId;
                if (!int.TryParse(id, out custId))
                {
                    continue;
                }

                var result = new Result()
                {
                    Code = 0
                };
                autoTransferService.UpdateAsync(custId, message.UplineId, message.IP, message.Language, message.State).Returns(result);
            }


            var expectedViewModel = new Result
            {
                Code = 0,
                Message = Resources.AutoTransfer_UpdateSuccess
            };


            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }


        [Theory]
        [InlineData(AutoTransferConstant.On)]
        [InlineData(AutoTransferConstant.Off)]
        public async Task HandlerUpdateMultipleQuery_CustIdsIsInvalid_ReturnsCorrectResults(string state)
        {
            // Arrange
            user.Id.Returns(1176070);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("en-US");

            var message = new UpdateMultipleQuery(user)
            {
                SearchAccountIds = "a,0, ",
                State = state
            };


            var expectedViewModel = new Result
            {
                Code = 0,
                Message = Resources.AutoTransfer_UpdateSuccess
            };

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        //TODO:check
        [Theory]
        [InlineData(AutoTransferConstant.On)]
        [InlineData(AutoTransferConstant.Off)]
        public async Task HandlerUpdateMultipleQuery_UpdateServiceNotSuccess_ReturnsCorrectResults(string state)
        {
            // Arrange
            user.Id.Returns(1176070);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("en-US");

            var message = new UpdateMultipleQuery(user)
            {
                SearchAccountIds = "1176083,33278659,14373883",
                State = state
            };

            var ids = message.SearchAccountIds.Split(new[] { ',' });
            var resultMessages = new List<string>();

            foreach (var id in ids)
            {
                int custId;
                int.TryParse(id, out custId);

                var result = new Result()
                {
                    Code = 1,
                    Message = "Error"
                };
                autoTransferService.UpdateAsync(custId, message.UplineId, message.IP, message.Language, message.State).Returns(result);

                resultMessages.Add(result.Message);

            }

            var expectedViewModel = new Result
            {
                Code = 1,
                Message = string.Join("<br />", resultMessages)
            };

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public async Task HandlerUpdateQuery_CustIdIsValid_ReturnsCorrectResult()
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
                Code = 0
            };
            autoTransferService.UpdateAsync(message.CustId, message.UplineId, message.IP, message.Language, message.State).Returns(result);

            var expectedViewModel = new Result
            {
                Code = 0,
                Message = Resources.AutoTransfer_UpdateSuccess
            };


            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }


        //TODO:check
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

            var expectedViewModel = await autoTransferService.GetTodayTransfersAsync(param);
            // Act


            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);

        }

        //TODO:check
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

            var expectedViewModel = await autoTransferService.GetHistoryTransfersAsync(param);
            // Act


            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);

        }



        //TODO:check
        [Fact]
        public async Task HandlerCancelJobQuery_StatusIsAllowed_ReturnsCorrectResult()
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

            var param = new CancelJobParam
            {
                FromCustId = message.UplineId,
                ToCustId = message.CustId
            };

            autoTransferService.CancelJobAsync(param).Returns(expectedViewModel);


            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }


        //TODO:check
        [Fact]
        public async Task HandlerCancelJobQuery_StatusIsNotAllowed_ReturnsCorrectResult()
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
                Code = 1,
            };

            var param = new CancelJobParam
            {
                FromCustId = message.UplineId,
                ToCustId = message.CustId
            };

            autoTransferService.CancelJobAsync(param).Returns(expectedViewModel);

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel.Code, actualViewModel.Code);
        }

    }
}
