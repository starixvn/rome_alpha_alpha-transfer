namespace Alpha.Transfer.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Alpha.Transfer.Criteria.TransferToDownlineHistory;
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Alpha.Transfer.Services;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
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
            var result = Tuple.Create(new List<TransferToDownlineHistoryModel>(), 0);
            //repository.GetTransferToDownlineHistory(Arg.Any<GetTransferToDownlineHistoryCriteria>()).Returns(result);

            var param = new TransferToDownlineHistoryPagingParam(1, string.Empty, new DateTime(), new DateTime(), 0, 500, 1);

            // Act
            var actualResult = serviceImpl.GetTransferToDownlineHistory(param);

            // Assert
            Assert.True(compareLogic.Compare(result.Item1, actualResult.Item1).AreEqual
                && result.Item2 == actualResult.Item2);
        }

        [Fact]
        public void GetTransferToDownlineHistory_HasData_ReturnsCorrectDataFromDataAccess()
        {
            // Arrange
            var result = Tuple.Create(new List<TransferToDownlineHistoryModel>
            {
                new TransferToDownlineHistoryModel(
                    new DateTime(2021, 1, 5, 3, 24, 33, 197),
                    "SUPERTESTTE010A",
                    0,
                    20,
                    10,
                    10,
                    "To SUPERTESTTE010A",
                    false),
                new TransferToDownlineHistoryModel(
                    new DateTime(2021, 1, 5, 3, 25, 33, 197),
                    "SUPERTESTTE010B",
                    1,
                    20,
                    10,
                    10,
                    "To SUPERTESTTE010B",
                    false)
            }, 2);

            //repository.GetTransferToDownlineHistory(Arg.Any<GetTransferToDownlineHistoryCriteria>()).Returns(result);

            var param = new TransferToDownlineHistoryPagingParam(1, string.Empty, new DateTime(), new DateTime(), 0, 500, 1);

            // Act
            var actualResult = serviceImpl.GetTransferToDownlineHistory(param);

            // Assert
            Assert.True(compareLogic.Compare(result.Item1, actualResult.Item1).AreEqual
                && result.Item2 == actualResult.Item2);
        }
    }
}