namespace Alpha.Transfer.Site.Tests.Handlers
{
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Alpha.Transfer.Services;
    using Alpha.Transfer.Site.Handlers;
    using Alpha.Transfer.Site.Requests.TransferToDownlineHistory;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class TransferToDownlineHistoryHandlerTests
    {
        private readonly TransferToDownlineHistoryService transferToDownlineHistoryService;

        private readonly TransferToDownlineHistoryPagingQuery query;
        private readonly TransferToDownlineHistoryPagingParam param;
        private readonly List<TransferToDownlineHistoryModel> transferToDownlines;
        private readonly int totalRecords = 3;

        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;

        private readonly TransferToDownlineHistoryHandler handler;

        public TransferToDownlineHistoryHandlerTests() : base()
        {
            userFixture = new UserFixture();
            compareLogic = new CompareLogic();

            transferToDownlineHistoryService = Substitute.For<TransferToDownlineHistoryService>();
            handler = new TransferToDownlineHistoryHandler(transferToDownlineHistoryService);

            query = new TransferToDownlineHistoryPagingQuery(userFixture.User)
            {
                UserName = string.Empty,
                FromDate = new DateTime(2021, 1, 11),
                ToDate = new DateTime(2021, 3, 11),
                PageSize = 500,
                PageIndex = 1
            };
            param = new TransferToDownlineHistoryPagingParam(
                query.CustId,
                query.UserName,
                query.FromDate,
                query.ToDate,
                query.PageSize,
                query.PageIndex);
            transferToDownlines = new List<TransferToDownlineHistoryModel>
            {
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 11), "Agent", 0, 1, 2, 3, "Desc 1"),
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 12), "Agent", 1, 2, 3, 4, string.Empty),
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 13), "Agent", 0, 3, 4, 5, "Desc 3")
            };
        }

        [Fact]
        public void Handle_HasNoData_ReturnsNoRecords()
        {
            // Arrange
            var result = Tuple.Create(new List<TransferToDownlineHistoryModel>(), 0);
            transferToDownlineHistoryService
                .GetTransferToDownlineHistory(Arg.Any<TransferToDownlineHistoryPagingParam>())
                .Returns(result);

            // Act
            var viewModel = handler.Handle(query);

            // Assert
            Assert.True(!viewModel.TransferToDownlines.Any() && viewModel.TotalRecords == 0);
        }

        [Fact]
        public void Handle_HasData_ReturnsCorrectData()
        {
            // Arrange
            var result = Tuple.Create(transferToDownlines, totalRecords);
            transferToDownlineHistoryService
                .GetTransferToDownlineHistory(Arg.Any<TransferToDownlineHistoryPagingParam>())
                .Returns(result);

            // Act
            var actualResult = handler.Handle(query);

            // Assert
            Assert.True(result.Item2 == actualResult.TotalRecords
                && compareLogic.Compare(result.Item1.ElementAt(0), actualResult.TransferToDownlines.ElementAt(0)).AreEqual
                && compareLogic.Compare(result.Item1.ElementAt(1), actualResult.TransferToDownlines.ElementAt(1)).AreEqual
                && compareLogic.Compare(result.Item1.ElementAt(2), actualResult.TransferToDownlines.ElementAt(2)).AreEqual);
        }
    }
}