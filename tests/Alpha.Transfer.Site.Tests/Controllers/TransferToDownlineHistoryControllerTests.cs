namespace Alpha.Transfer.Site.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using Alpha.Transfer.Configurations;
    using Alpha.Transfer.Models;
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using Alpha.Transfer.Services;
    using Alpha.Transfer.Site.Controllers;
    using Alpha.Transfer.Site.Requests.TransferToDownlineHistory;
    using Alpha.Transfer.Site.ViewModels;
    using Alpha.Transfer.Site.ViewModels.TransferToDownlineHistory;
    using MediatR;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "TransferToDownlineHistory")]
    public class TransferToDownlineHistoryControllerTests
    {
        private const string InvalidPageViewPath = "~/Views/Shared/InvalidPage.cshtml";
        private const string InvalidExcelFileName = "InvalidExcelFile.xls";

        private readonly IMediator mediator;
        private readonly IUser user;
        private readonly TransferService transferService;
        private readonly TransferToDownlineHistoryController controller;

        public TransferToDownlineHistoryControllerTests()
        {
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["TransferToDownlineHistory.OfferSites"] = "site"
            };

            mediator = Substitute.For<IMediator>();

            user = Substitute.For<IUser>();
            user.Language.Returns("en-US");
            user.LevelId.Returns((byte)3);
            user.Site.Returns("site");

            transferService = Substitute.For<TransferService>();

            controller = new TransferToDownlineHistoryController(mediator, transferService);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendQuery()
        {
            // Arrange
            var message = new TransferToDownlineHistoryQuery(user)
            {
                CustId = 1,
                CustName = "CustName",
                CustLevelId = 2
            };

            // Act
            controller.Index(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Index_NullQuery_ReturnsInvalidPage()
        {
            // Act
            var actualValue = controller.Index(null);

            // Assert
            Assert.Equal(InvalidPageViewPath, ((ViewResult)actualValue).ViewName);
        }

        [Fact]
        public void Index_InvalidQuery_ReturnsInvalidePage()
        {
            // Arrange
            var message = new TransferToDownlineHistoryQuery(user)
            {
                CustId = 1,
                CustName = "CustName",
                CustLevelId = 4
            };

            // Act
            var actualValue = controller.Index(message);

            // Assert
            Assert.Equal(InvalidPageViewPath, ((ViewResult)actualValue).ViewName);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendExcelQuery()
        {
            // Arrange
            var message = new TransferToDownlineHistoryExcelQuery(user)
            {
                CustId = 1,
                CustName = "CustName",
                CustLevelId = 2
            };

            mediator
                .Send(Arg.Any<TransferToDownlineHistoryExcelQuery>())
                .Returns(new ExcelFileViewModel("ExcelFileName", new byte[] { }));

            // Act
            controller.Excel(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void Excel_NullQuery_ReturnsInvalidExcelFile()
        {
            // Act
            var actualValue = controller.Excel(null) as FileContentResult;

            // Assert
            Assert.Equal(InvalidExcelFileName, actualValue.FileDownloadName);
        }

        [Fact]
        public void Excel_InvalidQuery_ReturnsInvalideExcelFile()
        {
            // Arrange
            var message = new TransferToDownlineHistoryExcelQuery(user)
            {
                CustId = 1,
                CustName = "CustName",
                CustLevelId = 4
            };

            // Act
            var actualValue = controller.Excel(message) as FileContentResult;

            // Assert
            Assert.Equal(InvalidExcelFileName, actualValue.FileDownloadName);
        }

        [Fact]
        public void GetTransferToDownlineHistoryData_Always_CallMediatorSendQuery()
        {
            // Arrange
            var message = new TransferToDownlineHistoryPagingQuery(user)
            {
                CustId = 1,
                CustName = "CustName",
                CustLevelId = 2
            };

            mediator.Send(Arg.Any<TransferToDownlineHistoryPagingQuery>())
                .Returns(new TransferToDownlineHistoryPagingViewModel(
                    new List<TransferToDownlineHistoryModel>(),
                    0));

            // Act
            controller.GetTransferToDownlineHistoryData(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }
    }
}