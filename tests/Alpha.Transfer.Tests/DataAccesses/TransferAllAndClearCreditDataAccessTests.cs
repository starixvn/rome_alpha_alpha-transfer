namespace Alpha.Transfer.Tests.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Database.TransferAllAndClearCredit;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Transfer.Criteria.TransferAllAndClearCredit;
    using Transfer.DataAccesses;
    using Transfer.Models.TransferAllAndClearCredit;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    [Collection("ObjectDbCollection")]
    public class TransferAllAndClearCreditDataAccessTests
    {
        private readonly ObjectDbFixture objectDbFixture;
        private readonly TransferAllAndClearCreditDataAccessImpl dataAccessImpl;
        private readonly CompareLogic compareLogic;

        public TransferAllAndClearCreditDataAccessTests(ObjectDbFixture objectDbFixture)
        {
            this.objectDbFixture = objectDbFixture;
            dataAccessImpl = new TransferAllAndClearCreditDataAccessImpl();
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetCalculateResult_NullDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var criteria = new GetCalculateResultCriteria(0, string.Empty);

            IDataReader dataReader = null;
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);

            var expectedData = new TransferResult(new List<TransferStep>(), new List<ClearCreditStep>(), new List<string>());

            // Act
            var actualData = dataAccessImpl.GetCalculateResult(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void GetCalculateResult_NoCredit_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new GetCalculateResultCriteria(0, string.Empty);

            var dataSet = TransferAllAndClearCreditDatabase.Calculation_NoCredit.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataSet);

            var transferSteps = new List<TransferStep>
            {
                new TransferStep(1, (decimal)1.2, 0),
                new TransferStep(2, (decimal)1.3, null)
            };

            var expectedData = new TransferResult(transferSteps, new List<ClearCreditStep>(), new List<string> { "Error 1" });

            // Act
            var actualData = dataAccessImpl.GetCalculateResult(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void GetCalculateResult_TransferCredit_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new GetCalculateResultCriteria(0, string.Empty);

            var dataSet = TransferAllAndClearCreditDatabase.Calculation_TransferCredit.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataSet);

            var transferSteps = new List<TransferStep>
            {
                new TransferStep(3, (decimal)1.2, 0),
                new TransferStep(4, (decimal)1.3, null)
            };

            var creditSteps = new List<ClearCreditStep>
            {
                new ClearCreditStep(1, (decimal)1.2, 0)
            };

            var expectedData = new TransferResult(transferSteps, creditSteps, new List<string> { "Error 2" });

            // Act
            var actualData = dataAccessImpl.GetCalculateResult(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void GetCalculateResult_NoTransferAndNoCredit_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new GetCalculateResultCriteria(0, string.Empty);

            var dataSet = TransferAllAndClearCreditDatabase.Calculation_NoTransferAndNoCredit.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataSet);

            var expectedData = new TransferResult(new List<TransferStep>(), new List<ClearCreditStep>(), new List<string> { "Error 3" });

            // Act
            var actualData = dataAccessImpl.GetCalculateResult(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void TransferAll_NullDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var criteria = new TransferAllCriteria(0, 1, 2, string.Empty, string.Empty, string.Empty, string.Empty, false);

            IDataReader dataReader = null;
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);
            objectDbFixture.ObjectDb.GetOuputParams().Returns(new Hashtable
            {
                ["ErrCode"] = 1,
                ["ErrMsg"] = string.Empty
            });

            var expectedData = new TransferAllResult(new List<TransferStep>(), 1, string.Empty, new List<string>());

            // Act
            var actualData = dataAccessImpl.TransferAll(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void TransferAll_HasErrorAndNoData_ReturnsCorrectData()
        {
            // Arrange
            var criteria = new TransferAllCriteria(0, 1, 2, string.Empty, string.Empty, string.Empty, string.Empty, false);

            var dataSet = TransferAllAndClearCreditDatabase.TransferAllHasErrorAndNoData.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataSet);
            objectDbFixture.ObjectDb.GetOuputParams().Returns(new Hashtable
            {
                ["ErrCode"] = 0,
                ["ErrMsg"] = string.Empty
            });

            var expectedData = new TransferAllResult(new List<TransferStep>(), 0, string.Empty, new List<string> { "Error 2" });

            // Act
            var actualData = dataAccessImpl.TransferAll(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void TransferAll_HasData_ReturnsCorrectData()
        {
            // Arrange
            var criteria = new TransferAllCriteria(0, 1, 2, string.Empty, string.Empty, string.Empty, string.Empty, false);

            var dataSet = TransferAllAndClearCreditDatabase.TransferAll.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataSet);
            objectDbFixture.ObjectDb.GetOuputParams().Returns(new Hashtable
            {
                ["ErrCode"] = 0,
                ["ErrMsg"] = string.Empty
            });

            var transferSteps = new List<TransferStep>
            {
                new TransferStep(1, (decimal)1.5, 0)
            };

            var expectedData = new TransferAllResult(transferSteps, 0, string.Empty, new List<string> { "Error 1" });

            // Act
            var actualData = dataAccessImpl.TransferAll(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void ClearCredit_NullDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var criteria = new ClearCreditCriteria(0, 1, 2, string.Empty, string.Empty, string.Empty);

            IDataReader dataReader = null;
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);
            objectDbFixture.ObjectDb.GetOuputParams().Returns(new Hashtable
            {
                ["ErrCode"] = 1,
                ["ErrMsg"] = string.Empty
            });

            var expectedData = new ClearCreditResult(new List<ClearCreditStep>(), 1, string.Empty);

            // Act
            var actualData = dataAccessImpl.ClearCredit(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void ClearCredit_HasData_ReturnsCorrectData()
        {
            // Arrange
            var criteria = new ClearCreditCriteria(0, 1, 2, string.Empty, string.Empty, string.Empty);

            var dataSet = TransferAllAndClearCreditDatabase.ClearCredit.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataSet);
            objectDbFixture.ObjectDb.GetOuputParams().Returns(new Hashtable
            {
                ["ErrCode"] = 0,
                ["ErrMsg"] = string.Empty
            });

            var transferSteps = new List<ClearCreditStep>
            {
                new ClearCreditStep(1, (decimal)1.2, 0)
            };

            var expectedData = new ClearCreditResult(transferSteps, 0, string.Empty);

            // Act
            var actualData = dataAccessImpl.ClearCredit(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }
    }
}
