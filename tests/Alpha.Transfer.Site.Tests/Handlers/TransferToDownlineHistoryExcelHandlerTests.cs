namespace Alpha.Transfer.Site.Tests.Handlers
{
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Alpha.Transfer.Params.TransferToDownlineHistory;
    using Alpha.Transfer.Services;
    using Alpha.Transfer.Site.Handlers;
    using Alpha.Transfer.Site.Requests.TransferToDownlineHistory;
    using Alpha.Transfer.Site.Tests.Fixtures;
    using Alpha.Transfer.Utils;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class TransferToDownlineHistoryExcelHandlerTests
    {
        private readonly TransferToDownlineHistoryService transferToDownlineHistoryService;
        private readonly TransferToDownlineHistoryExcelHandler handler;
        private readonly Workbook<TransferToDownlineHistoryModel> workbook;
        private readonly UserFixture userFixture;

        public TransferToDownlineHistoryExcelHandlerTests()
        {
            userFixture = new UserFixture();

            transferToDownlineHistoryService = Substitute.For<TransferToDownlineHistoryService>();
            workbook = Substitute.For<Workbook<TransferToDownlineHistoryModel>>();
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<TransferToDownlineHistoryModel>>(), Arg.Any<Mapper<TransferToDownlineHistoryModel>>()).Returns(workbook);

            handler = Substitute.ForPartsOf<TransferToDownlineHistoryExcelHandler>(transferToDownlineHistoryService, workbook);
            handler.MapPath = path => path;
            handler.IsFileExist = path => true;

            SystemTime.Today = new DateTime(2021, 1, 11);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Theory]
        [InlineData(4, "~/App_Data/ExcelTemplates/TransferToDownlineHistory/Master.xls")]
        [InlineData(3, "~/App_Data/ExcelTemplates/TransferToDownlineHistory/Agent.xls")]
        [InlineData(2, "~/App_Data/ExcelTemplates/TransferToDownlineHistory/Member.xls")]
        public void Handle_CustLevel_UseCorrectTemplate(byte levelId, string expectedTemplatePath)
        {
            // Arrange
            var message = CreateExcelQuery(levelId);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(tp => tp.TemplatePath == expectedTemplatePath));
        }

        [Fact]
        public void Handle_HasNoData_ExportExcelFileWithNoInformationText()
        {
            // Arrange
            transferToDownlineHistoryService
                .GetTransferToDownlineHistory(Arg.Any<TransferToDownlineHistoryParam>())
                .Returns(new List<TransferToDownlineHistoryModel>());
            var message = CreateExcelQuery(4);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(tp => tp.NoInfomationText == "No information is available"));
        }

        [Theory]
        [InlineData(null, "Transfer To Downline History 01/11/2021 -> 03/11/2021")]
        [InlineData("", "Transfer To Downline History 01/11/2021 -> 03/11/2021")]
        [InlineData("UserName", "Transfer To Downline History 01/11/2021 -> 03/11/2021 - Username: UserName")]
        public void Handle_Always_ExportCorrectSheetTitle(string userName, string expectedSheetTitle)
        {
            // Arrange
            var message = CreateExcelQuery(4, userName);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Title(Arg.Is(expectedSheetTitle));
        }

        [Fact]
        public void Handle_Always_ExportCorrectHeader()
        {
            // Arrange
            var message = CreateExcelQuery(4);

            // Act
            handler.Handle(message);

            // Arrange
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void Handle_Always_ExportCorrectExcelFileName()
        {
            // Arrange
            var message = CreateExcelQuery(4);

            // Act
            var excel = handler.Handle(message);

            // Assert
            Assert.Equal("Transfer To Downline History - 1112021.xls", excel.Name);
        }

        [Fact]
        public void GetBodyMapper_Always_ReturnsCorrectMapper()
        {
            // Arrange
            var viewModel = new TransferToDownlineHistoryModel(new DateTime(2021, 1, 11), "Agent", "Deposit", 1, 2, 3, "Desc 1");

            // Act
            var actualMapper = handler.GetBodyMapper;

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[Column.A](viewModel) == "01/11/2021 00:00:00"
                && (string)actualMapper.Mappings[Column.B](viewModel) == "Agent"
                && (string)actualMapper.Mappings[Column.C](viewModel) == "Deposit"
                && (decimal)actualMapper.Mappings[Column.D](viewModel) == 1
                && (decimal)actualMapper.Mappings[Column.E](viewModel) == 2
                && (decimal)actualMapper.Mappings[Column.F](viewModel) == 3
                && (string)actualMapper.Mappings[Column.G](viewModel) == "Desc 1");
        }

        private TransferToDownlineHistoryExcelQuery CreateExcelQuery(byte levelId, string userName = "")
            => new TransferToDownlineHistoryExcelQuery(userFixture.User)
            {
                CustLevelId = levelId,
                FromDate = new DateTime(2021, 1, 11),
                ToDate = new DateTime(2021, 3, 11),
                UserName = userName
            };
    }
}