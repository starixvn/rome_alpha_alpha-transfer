namespace Alpha.Transfer.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Alpha.Transfer.Services;
    using Alpha.Transfer.Site.Handlers;
    using Alpha.Transfer.Site.Requests.TransferToDownlineHistory;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class TransferToDownlineHistoryBaseHandlerTests
    {
        private const int pageSize = 500;
        private const int pageIndex = 1;

        private readonly DateTime fromDate = new DateTime(2021, 1, 11);
        private readonly DateTime toDate = new DateTime(2021, 3, 11);

        private readonly TransferToDownlineHistoryService transferToDownlineHistoryService;
        private readonly TransferToDownlineHistoryBaseHandler baseHandler;

        private readonly TransferToDownlineHistoryPagingQuery query;
        private readonly TransferToDownlineHistoryPagingParam param;
        private readonly IEnumerable<TransferToDownlineHistoryModel> transferToDownlines;
        private readonly int totalRecords = 3;

        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;

        public TransferToDownlineHistoryBaseHandlerTests()
        {
            userFixture = new UserFixture();
            compareLogic = new CompareLogic();

            transferToDownlineHistoryService = Substitute.For<TransferToDownlineHistoryService>();
            baseHandler = new TransferToDownlineHistoryBaseHandler(transferToDownlineHistoryService);
            query = new TransferToDownlineHistoryPagingQuery(userFixture.User)
            {
                UserName = string.Empty,
                FromDate = fromDate,
                ToDate = toDate,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            param = new TransferToDownlineHistoryPagingParam(
                query.CustId, 
                query.UserName, 
                query.FromDate, 
                query.ToDate, 
                query.PageSize, 
                query.PageIndex);
            transferToDownlines = new List<TransferToDownlineHistoryModel>
            {
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 11), "Agent", 0, 1, 2, 3, "Desc 1"),
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 12), "Agent", 1, 2, 3, 4, string.Empty),
                new TransferToDownlineHistoryModel(new DateTime(2021, 1, 13), "Agent", 0, 3, 4, 5, "Desc 3")
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
        public void GetTransferToDownlineHistory_HasData_ReturnsCorrectValue()
        {
            // Arrange
            var result = Tuple.Create(transferToDownlines, totalRecords);
            transferToDownlineHistoryService
                .GetTransferToDownlineHistory(Arg.Any<TransferToDownlineHistoryPagingParam>())
                .Returns(result);

            // Act
            var actualResult = baseHandler.GetTransferToDownlineHistory(param);

            // Assert
            Assert.True(result.Item2 == actualResult.Item2
                && compareLogic.Compare(result.Item1.ElementAt(0), actualResult.Item1.ElementAt(0)).AreEqual
                && compareLogic.Compare(result.Item1.ElementAt(1), actualResult.Item1.ElementAt(1)).AreEqual
                && compareLogic.Compare(result.Item1.ElementAt(2), actualResult.Item1.ElementAt(2)).AreEqual);
        }

        [Fact]
        public void GetTransferToDownlineHistory_HasNoData_ReturnsNoRecords()
        {
            // Arrange
            var result = Tuple.Create(Enumerable.Empty<TransferToDownlineHistoryModel>(), 0);
            transferToDownlineHistoryService
                .GetTransferToDownlineHistory(Arg.Any<TransferToDownlineHistoryPagingParam>())
                .Returns(result);

            // Act
            var actualResult = baseHandler.GetTransferToDownlineHistory(param);

            // Assert
            Assert.True(!actualResult.Item1.Any() && actualResult.Item2 == 0);
        }
    }
}