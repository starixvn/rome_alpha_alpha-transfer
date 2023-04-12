using System.Linq;

namespace Alpha.Transfer.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Transfer.Constants;
    using Alpha.Transfer.Criteria.CashBalanceTransfer;
    using Alpha.Transfer.Params.AutoTransfer;
    using Fanex.Data.Repository;
    using Fanex.Logging.Extensions.Database;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using Logging;
    using NSubstitute;
    using Transfer.Criteria.AutoTransfer;
    using Transfer.Models;
    using Transfer.Models.AutoTransfer;
    using Transfer.Services;
    using Xunit;

    [Trait("Category", " AutoTransfer")]
    [Collection("AnyValueCreatorCollection")]
    public class AutoTransferServiceImplTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly IDynamicRepository dynamicRepository;
        private readonly AutoTransferService serviceImpl;
        public enum Status
        {
            All,
            New,
            Waiting,
            Processing,
            Completed,
            Failed
        }
        public AutoTransferServiceImplTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
            LoggingConfig.Init(getDbExceptionInfo: DatabaseInfoGetters.GetDbExceptionInfo);

            dynamicRepository = Substitute.For<IDynamicRepository>();
            serviceImpl = new AutoTransferServiceImpl(dynamicRepository);
        }

        [Theory]
        [InlineData(AutoTransferConstant.On, true)]
        [InlineData(AutoTransferConstant.On, false)]
        [InlineData(AutoTransferConstant.Off, true)]
        [InlineData(AutoTransferConstant.Off, false)]
        public async Task UpdateAsync_ValidCustId_ReturnsCorrectResult(string state, bool isAutoJobRunning)
        {
            // Arrange

            var custId = 1176083;
            int creator = 1176083;
            string ip = "127.0.0.1";
            string languageKey = "E";


            dynamicRepository.ExecuteAsync(Arg.Any<GetJobStatusCriteria>()).Returns(x =>
            {
                ((GetJobStatusCriteria)x[0]).IsAutoJobRunning = isAutoJobRunning;
                return Task.CompletedTask;
            });

            var criteria = new UpdateMultipleCriteria(creator, custId, isAutoJobRunning, languageKey, ip, state);

            await dynamicRepository.ExecuteAsync(criteria);
            var messageResult = new Result
            {
                Code = 0,
                Message = string.Empty
            };


            var compareLogic = new CompareLogic();

            // Act
            var actualData = await serviceImpl.UpdateAsync(custId, creator, ip, languageKey, state);
            // Assert
            var result = compareLogic.Compare(messageResult, actualData);
            Assert.True(result.AreEqual);
        }

        [Theory]
        [InlineData(AutoTransferConstant.On, true)]
        [InlineData(AutoTransferConstant.On, false)]
        [InlineData(AutoTransferConstant.Off, true)]
        [InlineData(AutoTransferConstant.Off, false)]
        public async Task UpdateAsync_InvalidCustId_ReturnsCorrectResult(string state, bool isAutoJobRunning)
        {
            // Arrange

            var custId = 0;
            int creator = 1176083;

            dynamicRepository.ExecuteAsync(Arg.Any<GetJobStatusCriteria>()).Returns(x =>
            {
                ((GetJobStatusCriteria)x[0]).IsAutoJobRunning = isAutoJobRunning;
                return Task.CompletedTask;
            });

            string ip = "127.0.0.1";
            string languageKey = "E";
            var criteria = new UpdateMultipleCriteria(creator, custId, isAutoJobRunning, languageKey, ip, state);

            await dynamicRepository.ExecuteAsync(criteria);
            var messageResult = new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };


            var compareLogic = new CompareLogic();

            // Act
            var actualData = await serviceImpl.UpdateAsync(custId, creator, ip, languageKey, state);
            // Assert
            var result = compareLogic.Compare(messageResult, actualData);
            Assert.True(result.AreEqual);
        }

        //TODO: check
        [Theory]
        [InlineData(AutoTransferConstant.All)]
        [InlineData(AutoTransferConstant.New)]
        [InlineData(AutoTransferConstant.Waiting)]
        [InlineData(AutoTransferConstant.Processing)]
        [InlineData(AutoTransferConstant.Completed)]
        [InlineData(AutoTransferConstant.Failed)]
        public async Task GetTodayTransfersAsync_ValidCustId_ReturnsDataFromRepository(string status)
        {
            // Arrange

            var param = new GetTodayTransfersParam
            {
                CustId = 1176070,
                Username = "Master01",
                Status = status,
                PageIndex = 1,
                PageSize = 20,
                SortedBy = 0,
                Direction = 1
            };


            var transfers = new List<AutoTransferMonitoringModel>
                {
                    new AutoTransferMonitoringModel
                    {
                        Username="Master01",
                        FirstName="ma1",
                        TransferAmount=325000,
                        CustId=1,
                        Status=status,
                        Reason=string.Empty,
                        CompletedTime="01/05/2023 1:00:07 AM"
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master02",
                        FirstName="ma2",
                        TransferAmount=1800,
                        Status="Failed",
                        Reason="Due to system maintenance",
                        CustId=1,
                        CompletedTime="01/05/2023 1:00:08 AM"
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master03",
                        FirstName="cloe",
                        TransferAmount=2800,

                        CustId=1,
                        Status=status,
                        Reason="Due to unexpected issue",
                        CompletedTime="01/05/2023 1:00:08 AM"
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master04",
                        FirstName="da",
                        TransferAmount=3000,
                        CustId=1,
                        Status=status,
                        Reason="Due to manual transfer",
                        CompletedTime="01/05/2023 1:00:08 AM"
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master05",
                        FirstName="dud",
                        TransferAmount=(decimal)2000.21,
                        CustId=1,
                        Status=status,
                        Reason=string.Empty,
                        CompletedTime="01/05/2023 1:00:09 AM"
                    }
                };

            var pageModel = new AutoTransferMonitoringPageModel
            {
                Transfers = transfers,
                TotalItems = 5,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                SortedBy = param.SortedBy,
                Direction = param.Direction
            };

            dynamicRepository.FetchAsync<AutoTransferMonitoringModel>(Arg.Any<GetTodayTransfersCriteria>()).Returns(x =>
            {
                ((GetTodayTransfersCriteria)x[0]).TotalRow = 5;

                return Task.FromResult(transfers.AsEnumerable());
            });

            var compareLogic = new CompareLogic();

            // Act
            var actualData = await serviceImpl.GetTodayTransfersAsync(param);

            // Assert
            var result = compareLogic.Compare(pageModel, actualData);
            Assert.True(result.AreEqual);
        }

        //TODO: check
        [Theory]
        [InlineData(AutoTransferConstant.All)]
        [InlineData(AutoTransferConstant.New)]
        [InlineData(AutoTransferConstant.Waiting)]
        [InlineData(AutoTransferConstant.Processing)]
        [InlineData(AutoTransferConstant.Completed)]
        [InlineData(AutoTransferConstant.Failed)]
        public async Task GetHistoryTransfersAsync_ValidCustId_ReturnsDataFromRepository(string status)
        {
            // Arrange

            var param = new GetHistoryTransfersParam
            {
                CustId = 1176070,
                Username = "Master01",
                Status = status,
                PageIndex = 1,
                PageSize = 20,
                SortedBy = 0,
                Direction = 1,

                FromDate = new DateTime().AddDays(1).AddMonths(5).AddYears(2023),
                ToDate = new DateTime().AddDays(1).AddMonths(5).AddYears(2023),
            };

            var transfers = new List<AutoTransferMonitoringModel>
                {
                    new AutoTransferMonitoringModel
                    {
                        Username="Master01",
                        FirstName="ma1",
                        TransferAmount=325000,
                        CustId=1,
                        Status=status,
                        Reason=string.Empty,
                        CompletedTime="01/05/2023 1:00:07 AM"
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master02",
                        FirstName="ma2",
                        TransferAmount=1800,
                        Status="Failed",
                        Reason="Due to system maintenance",
                        CustId=1,
                        CompletedTime="01/05/2023 1:00:08 AM"
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master03",
                        FirstName="cloe",
                        TransferAmount=2800,

                        CustId=1,
                        Status=status,
                        Reason="Due to unexpected issue",
                        CompletedTime="01/05/2023 1:00:08 AM"
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master04",
                        FirstName="da",
                        TransferAmount=3000,
                        CustId=1,
                        Status=status,
                        Reason="Due to manual transfer",
                        CompletedTime="01/05/2023 1:00:08 AM"
                    },
                    new AutoTransferMonitoringModel
                    {
                        Username="Master05",
                        FirstName="dud",
                        TransferAmount=(decimal)2000.21,
                        CustId=1,
                        Status=status,
                        Reason=string.Empty,
                        CompletedTime="01/05/2023 1:00:09 AM"
                    }
                };
            var pageModel = new AutoTransferMonitoringPageModel
            {
                Transfers = transfers,
                TotalItems = 5,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                SortedBy = param.SortedBy,
                Direction = param.Direction
            };
            dynamicRepository.FetchAsync<AutoTransferMonitoringModel>(Arg.Any<GetHistoryTransfersCriteria>()).Returns(x =>
            {
                ((GetHistoryTransfersCriteria)x[0]).TotalRow = 5;

                return Task.FromResult(transfers.AsEnumerable());
            });
            var compareLogic = new CompareLogic();

            // Act
            var actualData = await serviceImpl.GetHistoryTransfersAsync(param);

            // Assert
            var result = compareLogic.Compare(pageModel, actualData);
            Assert.True(result.AreEqual);
        }
    }
}
