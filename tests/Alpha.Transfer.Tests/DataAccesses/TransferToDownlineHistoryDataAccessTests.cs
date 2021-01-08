namespace Alpha.Transfer.Tests.DataAccesses
{
    using Alpha.Transfer.Criteria.TransferToDownlineHistory;
    using Alpha.Transfer.DataAccesses;
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Database.TransferToDownlineHistory;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    [Collection("ObjectDbCollection")]
    public class TransferToDownlineHistoryDataAccessTests
    {
        private readonly ObjectDbFixture objectDbFixture;
        private readonly TransferToDownlineHistoryDataAccessImpl dataAccessImpl;
        private readonly CompareLogic compareLogic;

        public TransferToDownlineHistoryDataAccessTests(ObjectDbFixture objectDbFixture)
        {
            this.objectDbFixture = objectDbFixture;
            dataAccessImpl = new TransferToDownlineHistoryDataAccessImpl();
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetTransferToDownlineHistory_InValidCriteria_ReturnsEmptyCollection()
        {
            // Arrange
            var criteria = new GetTransferToDownlineHistoryCriteria(0, string.Empty, new DateTime(), new DateTime());

            // Act
            var collection = dataAccessImpl.GetTransferToDownlineHistory(criteria);

            // Assert
            Assert.Empty(collection);
        }

        [Fact]
        public void GetTransferToDownlineHistory_NoDataFromDatabase_ReturnsEmptyCollection()
        {
            // Arrange
            var criteria = new GetTransferToDownlineHistoryCriteria(1, string.Empty, new DateTime(), new DateTime());

            var dataSet = new DataTableReader(new DataTable());
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataSet);

            // Act
            var collection = dataAccessImpl.GetTransferToDownlineHistory(criteria);

            // Assert
            Assert.Empty(collection);
        }

        [Fact]
        public void GetTransferToDownlienHistory_HasDataFromDatabase_ReturnsCorrectData()
        {
            // Arrange
            var criteria = new GetTransferToDownlineHistoryCriteria(1, string.Empty, new DateTime(), new DateTime());

            var dataSet = TransferToDownlineHistoryDatabase.TransferToDownlineHistoryTable.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataSet);

            var expectedData = new List<TransferToDownlineHistoryModel>
            {
                new TransferToDownlineHistoryModel(
                    new DateTime(2021, 1, 5, 3, 24, 33, 197),
                    "SUPERTESTTE010A",
                    "Deposit",
                    20,
                    10,
                    10,
                    "To SUPERTESTTE010A"),
                new TransferToDownlineHistoryModel(
                    new DateTime(2021, 1, 5, 3, 25, 33, 197),
                    "SUPERTESTTE010B",
                    "Withdrawal",
                    20,
                    10,
                    10,
                    "To SUPERTESTTE010B")
            };

            // Act
            var actualData = dataAccessImpl.GetTransferToDownlineHistory(criteria).ToList();

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }
    }
}