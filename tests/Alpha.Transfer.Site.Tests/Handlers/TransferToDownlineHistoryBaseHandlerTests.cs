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
    using System.IO;
    using System.Linq;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class TransferToDownlineHistoryBaseHandlerTests
    {
        private const int pageSize = 500;
        private const int page = 1;

        private readonly DateTime fromDate = new DateTime(2021, 1, 11);
        private readonly DateTime toDate = new DateTime(2021, 3, 11);

        private readonly TransferToDownlineHistoryService transferToDownlineHistoryService;
        private readonly TransferToDownlineHistoryBaseHandler baseHandler;

        private readonly TransferToDownlineHistoryQuery query;
        private readonly TransferToDownlineHistoryParam param;
        private readonly IEnumerable<TransferToDownlineHistoryModel> records;

        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;

        public TransferToDownlineHistoryBaseHandlerTests()
        {
            userFixture = new UserFixture();
            compareLogic = new CompareLogic();

            transferToDownlineHistoryService = Substitute.For<TransferToDownlineHistoryService>();
            baseHandler = new TransferToDownlineHistoryBaseHandler(transferToDownlineHistoryService);
            query = new TransferToDownlineHistoryQuery(userFixture.User)
            {
                UserName = string.Empty,
                FromDate = fromDate,
                ToDate = toDate,
                PageSize = pageSize,
                Page = page
            };
            param = new TransferToDownlineHistoryParam(userFixture.User.Id, string.Empty, fromDate, toDate, pageSize, page);
            records = new List<TransferToDownlineHistoryModel>
            {
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 11), "Agent", "Deposit", 1, 2, 3, "Desc 1"),
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 12), "Agent", "Withdrawal", 2, 3, 4, string.Empty),
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 13), "Agent", "Deposit", 3, 4, 5, "Desc 3")
            };
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetExcelTemplatePath_FileNameIsNullOrWhiteSpace_ThrowsArgumentNullException(string fileName)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => baseHandler.GetExcelTemplatePath(fileName));
        }

        [Theory]
        [InlineData("filePath", "xls", "~/App_Data/ExcelTemplates/TransferToDownlineHistory/filePath.xls")]
        [InlineData("filePath1", "xls1", "~/App_Data/ExcelTemplates/TransferToDownlineHistory/filePath1.xls1")]
        public void MapExcelTemplatePath_ReturnCorrectFilePathWithExtension(
            string fileName,
            string extension,
            string expectedPath)
        {
            // Arrange
            baseHandler.MapPath = path => path;
            baseHandler.IsFileExist = path => true;

            // Act
            var actualPath = baseHandler.GetExcelTemplatePath(fileName, extension);

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }

        [Fact]
        public void MapExcelTemplatePath_TemplateFileDoesNotExist_ThrowFileNotFoundException()
        {
            // Arrange
            baseHandler.MapPath = path => path;
            baseHandler.IsFileExist = path => false;

            // Act && Assert
            Assert.Throws<FileNotFoundException>(() => baseHandler.GetExcelTemplatePath("reportName", "filename"));
        }

        [Fact]
        public void BuildTransferToDownlineHistoryParam_Always_ReturnsParamFromQuery()
        {
            // Act
            var actualParam = baseHandler.BuildTransferToDownlineHistoryParam(query);

            // Assert
            Assert.True(compareLogic.Compare(param, actualParam).AreEqual);
        }

        [Fact]
        public void GetTransferToDownlineHistory_HasData_ReturnsCorrectValue()
        {
            // Arrange
            transferToDownlineHistoryService
                .GetTransferToDownlineHistory(Arg.Any<TransferToDownlineHistoryParam>())
                .Returns(records);

            // Act
            var actualRecords = baseHandler.GetTransferToDownlineHistory(query);

            // Assert
            Assert.True(records.Count() == actualRecords.Count()
                && compareLogic.Compare(records.ElementAt(0), actualRecords.ElementAt(0)).AreEqual
                && compareLogic.Compare(records.ElementAt(1), actualRecords.ElementAt(1)).AreEqual
                && compareLogic.Compare(records.ElementAt(2), actualRecords.ElementAt(2)).AreEqual);
        }

        [Fact]
        public void GetTransferToDownlineHistory_HasNoData_ReturnsNoRecords()
        {
            // Arrange
            transferToDownlineHistoryService
                .GetTransferToDownlineHistory(Arg.Any<TransferToDownlineHistoryParam>())
                .Returns(new List<TransferToDownlineHistoryModel>());

            // Act
            var actualRecords = baseHandler.GetTransferToDownlineHistory(query);

            // Assert
            Assert.Empty(actualRecords);
        }
    }
}