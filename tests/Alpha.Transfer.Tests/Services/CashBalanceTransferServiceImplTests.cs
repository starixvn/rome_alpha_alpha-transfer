namespace Alpha.Transfer.Tests.Services
{
    using System;
    using System.Threading.Tasks;
    using AppResources;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using Transfer.Criteria;
    using Transfer.Criteria.CashBalanceTransfer;
    using Transfer.Models;
    using Transfer.Models.CashBalanceTransfer;
    using Transfer.Services;
    using Xunit;

    [Trait("Category", "CashBalanceTransfer")]
    [Collection("AnyValueCreatorCollection")]
    public class CashBalanceTransferServiceImplTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;
        private readonly CashBalanceTransferService serviceImpl;

        public CashBalanceTransferServiceImplTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;

            dynamicRepository = Substitute.For<IDynamicRepository>();
            cacheService = Substitute.For<ICacheService>();
            serviceImpl = new CashBalanceTransferServiceImpl(dynamicRepository, cacheService);
        }

        [Fact]
        public async Task GetCashBalanceTransferAsync_ValidCustId_ReturnsDataFromRepository()
        {
            // Arrange
            var custId = 1176083;
            var cashBalanceTransfer = new CashBalanceTransfer(
                custId,
                3,
                "master1",
                67599433.9247M,
                1101240.3283M,
                342552.00M,
                900000002.00M,
                0);

            dynamicRepository.GetAsync<CashBalanceTransfer>(Arg.Any<GetCashBalanceTransferCriteria>())
                .Returns(cashBalanceTransfer);

            var compareLogic = new CompareLogic();

            // Act
            var actualData = await serviceImpl.GetCashBalanceTransferAsync(custId);

            // Assert
            var result = compareLogic.Compare(cashBalanceTransfer, actualData);
            Assert.True(result.AreEqual);
        }

        [Theory]
        [InlineData(-1176083)]
        [InlineData(0)]
        [InlineData(1176070)]
        public void GetCashBalanceTransferAsync_InValidCustId_ThrowInvalidOperationException(int custId)
        {
            // Arrange
            dynamicRepository.GetAsync<CashBalanceTransfer>(new GetCashBalanceTransferCriteria(custId))
                .Throws(new InvalidOperationException());

            var cashBalanceTransferService = new CashBalanceTransferServiceImpl(dynamicRepository, cacheService);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await cashBalanceTransferService.GetCashBalanceTransferAsync(custId));
        }

        [Fact]
        public async Task AddCashBalanceTransferAsync_ValidParams_ReturnsTransferSuccessfully()
        {
            // Arrange
            var expectedResult = new Result
            {
                Message = Resources.TransferSuccessfully
            };

            dynamicRepository.ExecuteAsync(Arg.Any<AddCashBalanceTransferCriteria>())
                .Returns(Task.CompletedTask);

            // Act
            var actualData = await serviceImpl.AddCashBalanceTransferAsync(
                1176070,
                1176083,
                1101240.32M,
                1176070,
                "Admin1",
                0,
                "127.0.0.1",
                "E",
                string.Empty);

            // Assert
            Assert.True(actualData.Code == expectedResult.Code &&
                actualData.Message == expectedResult.Message);
        }

        [Theory]
        [InlineData(-1176070, 1176083, "127.0.0.1")]
        [InlineData(1176070, -1176083, "127.0.0.1")]
        [InlineData(1176070, 1176083, null)]
        public async Task AddCashBalanceTransferAsync_InvalidParams_ReturnsErrorCodeFromSP(int uplineId, int custId, string ip)
        {
            // Arrange
            var expectedResult = new Result();

            var criteria = new AddCashBalanceTransferCriteria(uplineId, custId, 0, uplineId, 0, ip, "E");

            dynamicRepository.ExecuteAsync(criteria)
                .Returns(Task.CompletedTask);

            // Act
            var actualData = await serviceImpl.AddCashBalanceTransferAsync(
                criteria.FromCustId,
                criteria.ToCustId,
                criteria.Amount,
                criteria.Creator,
                "Admin1",
                criteria.SubAccId,
                criteria.IP,
                criteria.Language,
                string.Empty);

            // Assert
            Assert.True(actualData.Code == expectedResult.Code);
        }

        [Fact]
        public async Task GetAllowTransfer_NoData_ReturnAllowTransferText()
        {
            //// Arrange
            dynamicRepository.ExecuteAsync(Arg.Any<GetTransferOptionCriteria>()).Returns(Task.CompletedTask);

            //// Act
            var act = await serviceImpl.GetAllowTransfer(263);

            //// Assert
            Assert.Equal(Resources.AllowTransfer, act);
        }

        [Fact]
        public async Task GetAllowTransfer_Daily_ReturnDailyText()
        {
            //// Arrange
            dynamicRepository.ExecuteAsync(Arg.Any<GetTransferOptionCriteria>()).Returns(x =>
            {
                ((GetTransferOptionCriteria)x[0]).TransferOption = "00000000";

                return Task.CompletedTask;
            });

            var expected = $"{Resources.AllowTransfer} {Resources.Daily}";

            //// Act
            var act = await serviceImpl.GetAllowTransfer(263);

            //// Assert
            Assert.Equal(expected, act);
        }

        [Theory]
        [InlineData("10000001", "Sunday")]
        [InlineData("11000001", "Monday - Sunday")]
        [InlineData("01000001", "Monday")]
        public async Task GetAllowTransfer_HasAllowTransfer_ReturnCorrectText(string transferOption, string text)
        {
            //// Arrange
            dynamicRepository.ExecuteAsync(Arg.Any<GetTransferOptionCriteria>()).Returns(x =>
            {
                ((GetTransferOptionCriteria)x[0]).TransferOption = transferOption;

                return Task.CompletedTask;
            });

            var expected = $"{Resources.AllowTransfer} {text}";

            //// Act
            var act = await serviceImpl.GetAllowTransfer(263);

            //// Assert
            Assert.Equal(expected, act);
        }

        [Theory]
        [InlineData("10000000")]
        [InlineData("11000000")]
        [InlineData("01000000")]
        public async Task GetTransferCondition_DailyTransfer_ReturnIsDaily(string transferOption)
        {
            //// Arrange
            dynamicRepository.ExecuteAsync(Arg.Any<GetTransferOptionCriteria>()).Returns(x =>
            {
                ((GetTransferOptionCriteria)x[0]).TransferOption = transferOption;

                return Task.CompletedTask;
            });

            //// Act
            var (isDaily, days) = await serviceImpl.GetTransferCondition(263);

            //// Assert
            Assert.True(isDaily);
        }

        [Theory]
        [InlineData("10000001", "Sunday")]
        [InlineData("11000001", "Monday - Sunday")]
        [InlineData("01000001", "Monday")]
        public async Task GetTransferCondition_HasAllowTransferAndNotDaily_ReturnCorrectText(string transferOption, string expectedValue)
        {
            //// Arrange
            dynamicRepository.ExecuteAsync(Arg.Any<GetTransferOptionCriteria>()).Returns(x =>
            {
                ((GetTransferOptionCriteria)x[0]).TransferOption = transferOption;

                return Task.CompletedTask;
            });

            //// Act
            var (isDaily, days) = await serviceImpl.GetTransferCondition(263);

            //// Assert
            Assert.Equal(expectedValue, string.Join(" - ", days));
        }
    }
}