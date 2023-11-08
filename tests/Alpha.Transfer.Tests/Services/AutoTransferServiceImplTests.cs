namespace Alpha.Transfer.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Transfer.Constants;
    using Alpha.Transfer.Params.AutoTransfer;
    using Fanex.Data.Repository;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
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
        private readonly CompareLogic compareLogic;

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
            dynamicRepository = Substitute.For<IDynamicRepository>();
            serviceImpl = new AutoTransferServiceImpl(dynamicRepository);
            compareLogic = new CompareLogic();
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
                var criteria = (GetJobStatusCriteria)x[0];
                if (criteria != null)
                {
                    criteria.IsAutoJobRunning = isAutoJobRunning;
                }

                return Task.CompletedTask;
            });

            var messageResult = new Result
            {
                Code = 0,
                Message = string.Empty
            };

            var param = new UpdateSingleParam
            {
                CustId = custId,
                UplineId = creator,
                IP = ip,
                Language = languageKey,
                State = state,
                Schedule = new[] { "1" },
            };

            // Act
            var actualData = await serviceImpl.UpdateAsync(param);

            // Assert
            Assert.True(compareLogic.Compare(messageResult, actualData).AreEqual);
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

            dynamicRepository.ExecuteAsync(Arg.Any<UpdateStatusCriteria>()).Returns(x =>
            {
                var criteria = (UpdateStatusCriteria)x[0];
                if (criteria != null)
                {
                    criteria.ErrCode = 1;
                    criteria.ErrMsg = "";
                }

                return Task.CompletedTask;
            });

            dynamicRepository.ExecuteAsync(Arg.Any<GetJobStatusCriteria>()).Returns(x =>
            {
                var criteria = (GetJobStatusCriteria)x[0];
                if (criteria != null)
                {
                    criteria.IsAutoJobRunning = isAutoJobRunning;
                }

                return Task.CompletedTask;
            });

            string ip = "127.0.0.1";
            string languageKey = "E";
            var messageResult = new Result
            {
                Code = 1,
                Message = ""
            };

            var param = new UpdateSingleParam
            {
                CustId = custId,
                UplineId = creator,
                IP = ip,
                Language = languageKey,
                State = state,
                Schedule = new string[] { },
            };

            // Act
            var actualData = await serviceImpl.UpdateAsync(param);

            // Assert
            Assert.True(compareLogic.Compare(messageResult, actualData).AreEqual);
        }

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
                var criteria = (GetTodayTransfersCriteria)x[0];
                if (criteria != null)
                {
                    criteria.TotalRow = 5;
                }

                return Task.FromResult(transfers.AsEnumerable());
            });

            var compareLogic = new CompareLogic();

            // Act
            var actualData = await serviceImpl.GetTodayTransfersAsync(param);

            // Assert
            Assert.True(compareLogic.Compare(pageModel, actualData).AreEqual);
        }

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
                var criteria = (GetHistoryTransfersCriteria)x[0];

                if (criteria != null)
                {
                    criteria.TotalRow = 5;
                }

                return Task.FromResult(transfers.AsEnumerable());
            });
            var compareLogic = new CompareLogic();

            // Act
            var actualData = await serviceImpl.GetHistoryTransfersAsync(param);

            // Assert
            Assert.True(compareLogic.Compare(pageModel, actualData).AreEqual);
        }

        [Fact]
        public async Task CancelJobAsync_Success_ReturnsSuccessResult()
        {
            // Arrange
            dynamicRepository.ExecuteAsync(Arg.Any<CancelJobCriteria>()).Returns(x =>
            {
                var criteria = (CancelJobCriteria)x[0];
                if (criteria != null)
                {
                    criteria.ErrCode = 0;
                    criteria.ErrMsg = "";
                }

                return Task.CompletedTask;
            });

            var param = new CancelJobParam();
            var expectedValue = new Result { Code = 0, Message = "" };

            // Act
            var actualValue = await serviceImpl.CancelJobAsync(param);

            // Assert
            Assert.True(compareLogic.Compare(expectedValue, actualValue).AreEqual);
        }

        [Fact]
        public async Task CancelJobAsync_Failed_ReturnsFalseResult()
        {
            // Arrange
            dynamicRepository.ExecuteAsync(Arg.Any<CancelJobCriteria>()).Returns(x =>
            {
                var criteria = (CancelJobCriteria)x[0];
                if (criteria != null)
                {
                    criteria.ErrCode = 1;
                    criteria.ErrMsg = "";
                }

                return Task.CompletedTask;
            });

            var param = new CancelJobParam();
            var expectedValue = new Result { Code = 1, Message = "" };

            // Act
            var actualValue = await serviceImpl.CancelJobAsync(param);

            // Assert
            Assert.True(compareLogic.Compare(expectedValue, actualValue).AreEqual);
        }
    }
}