namespace Alpha.Transfer.Tests.Services
{
    using Alpha.Transfer.Criteria.TransferToDownlineHistory;
    using Fanex.Data.Repository;
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Alpha.Transfer.Services;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class TransferToDownlineHistoryServiceImplTests
    {
        private readonly IDynamicRepository repository;
        private readonly TransferToDownlineHistoryServiceImpl serviceImpl;
        private readonly CompareLogic compareLogic;

        public TransferToDownlineHistoryServiceImplTests()
        {
            repository = Substitute.For<IDynamicRepository>();
            serviceImpl = new TransferToDownlineHistoryServiceImpl(repository);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetTransferToDownlineHistory_NoData_ReturnsEmptyDataFromDataAccess()
        {
            // Arrange
            var data = new List<TransferToDownlineHistoryModel>();
            repository.Fetch<TransferToDownlineHistoryModel>(Arg.Any<GetTransferToDownlineHistoryCriteria>()).Returns(data);

            var param = new TransferToDownlineHistoryParam(1, string.Empty, new DateTime(), new DateTime());

            // Act
            var actualData = serviceImpl.GetTransferToDownlineHistory(param).ToList();

            // Assert
            Assert.True(compareLogic.Compare(data, actualData).AreEqual);
        }

        [Fact]
        public void GetTransferToDownlineHistory_HasData_ReturnsCorrectDataFromDataAccess()
        {
            // Arrange
            var data = new List<TransferToDownlineHistoryModel>
            {
                new TransferToDownlineHistoryModel(
                    new DateTime(2021, 1, 5, 3, 24, 33, 197),
                    "SUPERTESTTE010A",
                    0,
                    20,
                    10,
                    10,
                    "To SUPERTESTTE010A"),
                new TransferToDownlineHistoryModel(
                    new DateTime(2021, 1, 5, 3, 25, 33, 197),
                    "SUPERTESTTE010B",
                    1,
                    20,
                    10,
                    10,
                    "To SUPERTESTTE010B")
            };

            repository.Fetch<TransferToDownlineHistoryModel>(Arg.Any<GetTransferToDownlineHistoryCriteria>()).Returns(data);

            var param = new TransferToDownlineHistoryParam(1, string.Empty, new DateTime(), new DateTime());

            // Act
            var actualData = serviceImpl.GetTransferToDownlineHistory(param).ToList();

            // Assert
            Assert.True(compareLogic.Compare(data, actualData).AreEqual);
        }
    }
}