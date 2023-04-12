using Alpha.Transfer.Constants;
using Alpha.Transfer.Site.Requests.AutoTransfer;
using Alpha.Transfer.Site.ViewModels.AutoTransfer;

namespace Alpha.Transfer.Site.Tests.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AppResources;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Sunplus.Agent.Model.Entities;
    using Transfer.Models;
    using Xunit;

    [Trait("Category", "AutoTransfer")]
    public class AutoTransferControllerTests
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;
        private readonly IUser user;
        private readonly AutoTransferController controller;

        public AutoTransferControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            user = Substitute.For<IUser>();
            user.Id.Returns(1);
            controller = new AutoTransferController(mediator, userSession);
        }

        #region Update


        [Fact]
        public async Task Update_QueryIsNull_ReturnBadRequestResult()
        {
            // Arrange

            var expectedResult = Result.CreateErrorResult(Resources.BadRequest, 400);

            // Act
            var actualResult = await controller.Update(null) as JsonResult;

            // Assert
            var actualData = (Result)actualResult.Data;

            Assert.True(actualData.Code == expectedResult.Code &&
                        actualData.Message == expectedResult.Message);
        }




        [Fact]
        public async Task Update_Always_CallsMediatorUpdateQuery()
        {
            // Arrange
            var message = new UpdateQuery(user) { CustId = 1176083, };

            // Act
            await controller.Update(message);

            // Assert
            await mediator.Received().SendAsync(message);
        }


        [Theory]
        [InlineData(-1176083, 1176070)]
        [InlineData(1176083, 0)]
        [InlineData(0, 0)]
        public async Task Update_QueryIsNotValid_ReturnBadRequestResult(int custId, int uplineId)
        {
            // Arrange
            user.Id.Returns(uplineId);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");

            var query = new UpdateQuery(user)
            {
                CustId = custId,
            };

            var expectedResult = Result.CreateErrorResult(Resources.BadRequest, 400);

            // Act
            var actualResult = await controller.Update(query) as JsonResult;

            // Assert
            var actualData = (Result)actualResult.Data;

            Assert.True(actualData.Code == expectedResult.Code &&
                actualData.Message == expectedResult.Message);
        }

        [Fact]
        public async Task Update_QueryIsInternal_ReturnNotAuthorizedResult()
        {
            // Arrange
            user.Id.Returns(1176070);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");

            var query = new UpdateQuery(user)
            {
                CustId = 1176083,
            };

            var expectedResult = Result.CreateErrorResult(Resources.NotAuthorized, 403);

            userSession.IsInternal.Returns(true);

            // Act
            var actualResult = await controller.Update(query) as JsonResult;

            // Assert
            var actualData = (Result)actualResult.Data;

            Assert.True(actualData.Code == expectedResult.Code &&
                        actualData.Message == expectedResult.Message);
        }

        #endregion


        #region MultipleUpdate

        [Fact]
        public async Task UpdateMultiple_Always_CallsMediatorUpdateMultipleQuery()
        {
            // Arrange
            user.Id.Returns(1176070);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");


            var message = new UpdateMultipleQuery(user)
            {
                SearchAccountIds = "1176083,33278659,14373487",
            };
            // Act
            await controller.UpdateMultiple(message);

            // Assert
            await mediator.Received().SendAsync(Arg.Any<GetTransferConditionQuery>());
        }

        #endregion


        #region PostUpdateMultiple


        [Fact]
        public async Task PostUpdateMultiple_Always_CallsMediatorUpdateQuery()
        {
            // Arrange
            var message = new UpdateMultipleQuery(user)
            {
                SearchAccountIds = "1176083,33278659,14373487",
            };

            // Act
            await controller.PostUpdateMultiple(message);

            // Assert
            await mediator.Received().SendAsync(message);
        }


        [Fact]
        public async Task PostUpdateMultiple_QueryIsNull_ReturnBadRequestResult()
        {
            // Arrange

            var expectedResult = Result.CreateErrorResult(Resources.BadRequest, 400);

            // Act
            var actualResult = await controller.PostUpdateMultiple(null) as JsonResult;

            // Assert
            var actualData = (Result)actualResult.Data;

            Assert.True(actualData.Code == expectedResult.Code &&
                        actualData.Message == expectedResult.Message);
        }

        [Theory]
        [InlineData("1176083,33278659,14373487", -1176070)]
        [InlineData("", 1176070)]
        [InlineData("1176083,33278659,14373487", 0)]
        public async Task PostUpdateMultiple_QueryIsNotValid_ReturnBadRequestResult(string searchAccountIds, int uplineId)
        {
            // Arrange
            user.Id.Returns(uplineId);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");

            var query = new UpdateMultipleQuery(user)
            {
                SearchAccountIds = searchAccountIds,
            };

            var expectedResult = Result.CreateErrorResult(Resources.BadRequest, 400);

            // Act
            var actualResult = await controller.PostUpdateMultiple(query) as JsonResult;

            // Assert
            var actualData = (Result)actualResult.Data;

            Assert.True(actualData.Code == expectedResult.Code &&
                actualData.Message == expectedResult.Message);
        }

        [Fact]
        public async Task PostUpdateMultiple_QueryIsInternal_ReturnNotAuthorizedResult()
        {
            // Arrange
            user.Id.Returns(1176070);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");

            var query = new UpdateMultipleQuery(user)
            {
                SearchAccountIds = "1176083,33278659,14373487"
            };

            var expectedResult = Result.CreateErrorResult(Resources.NotAuthorized, 403);

            userSession.IsInternal.Returns(true);

            // Act
            var actualResult = await controller.PostUpdateMultiple(query) as JsonResult;

            // Assert
            var actualData = (Result)actualResult.Data;

            Assert.True(actualData.Code == expectedResult.Code &&
                        actualData.Message == expectedResult.Message);
        }

        #endregion


        [Fact]
        public async Task GetTodayTransfers_Always_CallsMediatorGetTodayTransfers()
        {
            // Arrange
            user.Id.Returns(1176070);

            var message = new GetTodayTransfersQuery(user);

            // Act
            await controller.GetTodayTransfers(message);

            // Assert
            await mediator.Received().SendAsync(message);
        }


        [Fact]
        public async Task GetHistoryTransfers_Always_CallsMediatorGetHistoryTransfers()
        {
            // Arrange
            user.Id.Returns(1176070);

            var message = new GetHistoryTransfersQuery(user);

            // Act
            await controller.GetHistoryTransfers(message);

            // Assert
            await mediator.Received().SendAsync(message);
        }


        #region Cancel Job

        [Fact]
        public async Task CancelJob_Always_CallsMediatorGetHistoryTransfers()
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

            // Act
            await controller.CancelJob(message);

            // Assert
            await mediator.Received().SendAsync(message);
        }


        [Fact]
        public async Task CancelJob_QueryIsNull_ReturnBadRequestResult()
        {
            // Arrange

            var expectedResult = Result.CreateErrorResult(Resources.BadRequest, 400);

            // Act
            var actualResult = await controller.CancelJob(null) as JsonResult;

            // Assert
            var actualData = (Result)actualResult.Data;

            Assert.True(actualData.Code == expectedResult.Code &&
                        actualData.Message == expectedResult.Message);
        }

        [Theory]
        [InlineData(-1176083, 1176070)]
        [InlineData(1176083, -1176070)]
        [InlineData(0, 0)]
        public async Task CancelJob_QueryIsNotValid_ReturnBadRequestResult(int custId, int uplineId)
        {
            // Arrange
            user.Id.Returns(uplineId);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");

            var query = new CancelJobQuery(user)
            {
                CustId = custId,
            };

            var expectedResult = Result.CreateErrorResult(Resources.BadRequest, 400);

            // Act
            var actualResult = await controller.CancelJob(query) as JsonResult;

            // Assert
            var actualData = (Result)actualResult.Data;

            Assert.True(actualData.Code == expectedResult.Code &&
                        actualData.Message == expectedResult.Message);
        }

        [Fact]
        public async Task CancelJob_QueryIsInternal_ReturnNotAuthorizedResult()
        {
            // Arrange
            user.Id.Returns(1176070);
            user.SubAccId.Returns(0);
            user.IP.Returns("127.0.0.1");
            user.LanguageKey.Returns("E");

            var query = new CancelJobQuery(user)
            {
                CustId = 1176083,
            };

            var expectedResult = Result.CreateErrorResult(Resources.NotAuthorized, 403);

            userSession.IsInternal.Returns(true);

            // Act
            var actualResult = await controller.CancelJob(query) as JsonResult;

            // Assert
            var actualData = (Result)actualResult.Data;

            Assert.True(actualData.Code == expectedResult.Code &&
                        actualData.Message == expectedResult.Message);
        }

        #endregion


    }
}
