namespace Alpha.Transfer.Tests.DataAccesses
{
    using Alpha.Transfer.Params.MultipleTransfer;
    using Database.TransferAllAndClearCredit;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Transfer.Criteria.MultipleTransfer;
    using Transfer.DataAccesses;
    using Transfer.Models;
    using Transfer.Models.MultipleTransfer;
    using Xunit;

    [Trait("Category", "MultipleTransfer")]
    [Collection("ObjectDbCollection")]
    public class MultipleTransferDataAccessImplTests
    {
        private readonly ObjectDbFixture objectDbFixture;
        private readonly MultipleTransferDataAccessImpl dataAccessImpl;
        private readonly CompareLogic compareLogic;

        public MultipleTransferDataAccessImplTests(ObjectDbFixture objectDbFixture)
        {
            this.objectDbFixture = objectDbFixture;
            dataAccessImpl = new MultipleTransferDataAccessImpl();
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetCustomerList_NullDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var param = new GetCustomerListParam(UserLevel.Agent, 1, string.Empty, 1, 1, false, false, 2);
            var criteria = new GetAgentTransferListCriteria(param);

            IDataReader dataReader = null;
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);

            var expectedData = new CustomerListModel(new List<CustomerInformation>(), 0);

            // Act
            var actualData = dataAccessImpl.GetCustomerList(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void GetCustomerList_HasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var param = new GetCustomerListParam(UserLevel.Agent, 1, string.Empty, 1, 1, false, false, 2);
            var criteria = new GetAgentTransferListCriteria(param);

            var dataSet = MultipleTransferDatabase.CustomerList.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataSet);

            var customers = new List<CustomerInformation>
            {
                new CustomerInformation
                {
                    Username = "username",
                    YesterdayBalance = (decimal)1.2,
                    Balance =(decimal)1.3,
                    YesterdayCashBalance = (decimal)1.4,
                    CashBalance =(decimal)1.5,
                    Outstanding = (decimal)1.6,
                    GivenCredit = (decimal)1.7,
                    UsedCredit = (decimal)1.8,
                    FirstName = "firstname",
                    LastName = "lastname",
                    CustId = 2,
                    Enabled = false,
                    Closed = true,
                    Suspended = true,
                    ClosedReason = "reason"
                }
            };

            var expectedData = new CustomerListModel(customers, 0);

            // Act
            var actualData = dataAccessImpl.GetCustomerList(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }
    }
}