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

        private readonly TransferToDownlineHistoryQuery query;
        private readonly IEnumerable<TransferToDownlineHistoryModel> allRecords;

        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;

        private readonly TransferToDownlineHistoryHandler handler;

        private readonly IEnumerable<TransferToDownlineHistoryModel> pagingRecords;

        public TransferToDownlineHistoryHandlerTests() : base()
        {
            userFixture = new UserFixture();
            compareLogic = new CompareLogic();

            transferToDownlineHistoryService = Substitute.For<TransferToDownlineHistoryService>();
            handler = new TransferToDownlineHistoryHandler(transferToDownlineHistoryService);

            query = new TransferToDownlineHistoryQuery(userFixture.User)
            {
                UserName = string.Empty,
                FromDate = new DateTime(2021, 1, 11),
                ToDate = new DateTime(2021, 3, 11),
                PageSize = 2,
                Page = 1
            };
            allRecords = new List<TransferToDownlineHistoryModel>
            {
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 11), "Agent", "Deposit", 1, 2, 3, "Desc 1"),
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 12), "Agent", "Withdrawal", 2, 3, 4, string.Empty),
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 13), "Agent", "Deposit", 3, 4, 5, "Desc 3")
            };
            pagingRecords = new List<TransferToDownlineHistoryModel>
            {
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 11), "Agent", "Deposit", 1, 2, 3, "Desc 1"),
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 12), "Agent", "Withdrawal", 2, 3, 4, string.Empty)
            };
        }

        [Fact]
        public void Handle_HasNoData_ReturnsNoRecords()
        {
            // Arrange
            transferToDownlineHistoryService
                .GetTransferToDownlineHistory(Arg.Any<TransferToDownlineHistoryParam>())
                .Returns(new List<TransferToDownlineHistoryModel>());

            // Act
            var viewModel = handler.Handle(query);

            // Assert
            Assert.Empty(viewModel.TransferToDownlineHistories);
        }

        [Fact]
        public void Handle_HasData_ReturnsCorrectData()
        {
            // Arrange
            transferToDownlineHistoryService
                .GetTransferToDownlineHistory(Arg.Any<TransferToDownlineHistoryParam>())
                .Returns(allRecords);

            // Act
            var actualPagingRecords = handler.Handle(query).TransferToDownlineHistories;

            // Assert
            Assert.True(pagingRecords.Count() == actualPagingRecords.Count()
                && compareLogic.Compare(pagingRecords.ElementAt(0), actualPagingRecords.ElementAt(0)).AreEqual
                && compareLogic.Compare(pagingRecords.ElementAt(1), actualPagingRecords.ElementAt(1)).AreEqual);
        }

        [Fact]
        public void Handle_HasNoData_ReturnsTotalRecordsEqualZero()
        {
            // Arrange
            transferToDownlineHistoryService
                .GetTransferToDownlineHistory(Arg.Any<TransferToDownlineHistoryParam>())
                .Returns(new List<TransferToDownlineHistoryModel>());

            // Act
            var viewModel = handler.Handle(query);

            // Assert
            Assert.Equal(0, viewModel.TotalRecords);
        }

        [Fact]
        public void Handle_HasData_ReturnsCorrectTotalRecords()
        {
            // Arrange
            transferToDownlineHistoryService
                .GetTransferToDownlineHistory(Arg.Any<TransferToDownlineHistoryParam>())
                .Returns(allRecords);

            // Act
            var viewModel = handler.Handle(query);

            // Assert
            Assert.Equal(allRecords.Count(), viewModel.TotalRecords);
        }
    }
}