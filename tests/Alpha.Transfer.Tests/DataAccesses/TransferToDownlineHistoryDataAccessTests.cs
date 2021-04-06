namespace Alpha.Transfer.Tests.DataAccesses
{
    using Alpha.Transfer.Criteria.TransferToDownlineHistory;
    using Alpha.Transfer.DataAccesses;
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Constants;
    using Database.TransferToDownlineHistory;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
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
            var criteria = BuildCriteria(0);

            // Act
            var result = dataAccessImpl.GetTransferToDownlineHistory(criteria);

            // Assert
            Assert.Empty(result.Item1);
        }

        [Fact]
        public void GetTransferToDownlineHistory_NoDataFromDatabase_ReturnsEmptyCollection()
        {
            // Arrange
            var criteria = BuildCriteria(1);

            var dataSet = new DataTableReader(new DataTable());
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataSet);

            // Act
            var result = dataAccessImpl.GetTransferToDownlineHistory(criteria);

            // Assert
            Assert.Empty(result.Item1);
        }

        [Fact]
        public void GetTransferToDownlienHistory_HasDataFromDatabase_ReturnsCorrectData()
        {
            // Arrange
            var criteria = BuildCriteria(1);

            var dataSet = TransferToDownlineHistoryDatabase.TransferToDownlineHistoryTable.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataSet);

            var expectedData = new List<TransferToDownlineHistoryModel>
            {
                new TransferToDownlineHistoryModel(
                    new DateTime(2021, 1, 5, 3, 24, 33, 197),
                    "SUPERTESTTE010A",
                    1,
                    20,
                    10,
                    10,
                    "To SUPERTESTTE010A"),
                new TransferToDownlineHistoryModel(
                    new DateTime(2021, 1, 5, 3, 25, 33, 197),
                    "SUPERTESTTE010B",
                    0,
                    20,
                    10,
                    10,
                    "To SUPERTESTTE010B")
            };

            // Act
            var actualData = dataAccessImpl.GetTransferToDownlineHistory(criteria).Item1;

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        private GetTransferToDownlineHistoryCriteria BuildCriteria(int custId)
            => new GetTransferToDownlineHistoryCriteria(
                custId,
                string.Empty,
                new DateTime(),
                new DateTime(),
                DefaultVariablesTest.PageSize,
                DefaultVariablesTest.PageIndex);
    }
}